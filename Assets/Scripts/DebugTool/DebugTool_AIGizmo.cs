using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Common;
using Common.Tables;
using Common.Log;

public class DebugTool_AIGizmo : MonoBehaviour {

    #if GAME_AI_ONLY
    private LLTeam redTeam = null;
    private LLTeam blueTeam = null;
    public List<Vector3D> CloseMarkWithoutBallPos = new List<Vector3D> (); 

    private double interceptRunRate;
    private double shortPassLimit;
    private double headShootLimit;
    
    private bool NeedAttackSupport = false;
    private bool NeedBallTracking = true;
    private bool NeedInterceptRange = true;
    private bool NeedAttackerHomePos = false;
    private bool NeedDefenderHomePos = false;
    private bool NeedMarkWithBallTrack = true;
    private bool NeedMarkWithoutBallTrack = true;
	// Use this for initialization
	void Start () {
        PLDirector pld = gameObject.GetComponent<PLDirector>();
        redTeam = LLDirector.Instance.Scene.RedTeam;
        blueTeam = LLDirector.Instance.Scene.BlueTeam;

        interceptRunRate = TableManager.Instance.AIConfig.GetItem("speed_rate_block").Value;        
        shortPassLimit = TableManager.Instance.AIConfig.GetItem("long_distance_pass").Value;
        headShootLimit = TableManager.Instance.AIConfig.GetItem("head_shoot_distance").Value;
	}

    void OnGUI()
    {
        GUILayout.BeginArea(new Rect(10 , Screen.height / 2 + 10, Screen.width/2 - 20, Screen.height / 4));
        GUILayout.BeginVertical();

        NeedBallTracking = GUILayout.Toggle(NeedBallTracking, "显示球轨迹");
        NeedInterceptRange = GUILayout.Toggle(NeedInterceptRange, "显示拦截");
        NeedAttackSupport = GUILayout.Toggle(NeedAttackSupport, "显示接应");
        NeedAttackerHomePos = GUILayout.Toggle(NeedAttackerHomePos, "显示攻方HomePos");
        NeedDefenderHomePos = GUILayout.Toggle(NeedDefenderHomePos, "显示守方HomePos");
        NeedMarkWithBallTrack = GUILayout.Toggle(NeedMarkWithBallTrack, "显示有球盯防");
        NeedMarkWithoutBallTrack = GUILayout.Toggle(NeedMarkWithoutBallTrack, "显示无球盯防");

        GUILayout.EndVertical();
        GUILayout.EndArea();
    }

    void OnDrawGizmos()
    {
        LLTeam attackTeam = null;
        LLTeam defendTeam = null;
        if (redTeam == null || blueTeam == null)
            return;
        if (redTeam.State == Common.ETeamState.TS_ATTACK)
        {
            attackTeam = redTeam;
            defendTeam = blueTeam;
        }
        else
        {
            attackTeam = blueTeam;
            defendTeam = redTeam;
        }


        //for ball
        LLBall ball = LLDirector.Instance.Scene.Ball;
        if (NeedBallTracking)
        {
            Gizmos.color = Color.black;
            Gizmos.DrawLine(new Vector3((float)ball.GetPosition().X,
                                        (float)ball.GetPosition().Y,
                                        (float)ball.GetPosition().Z),
                            new Vector3((float)ball.TargetPos.X,
                                        (float)ball.TargetPos.Y,
                                        (float)ball.TargetPos.Z));

            Gizmos.DrawSphere(new Vector3((float)ball.TargetPos.X,
                                          (float)ball.TargetPos.Y,
                                          (float)ball.TargetPos.Z), 0.25f);
        }
        if(NeedInterceptRange)
        {
            if (ball.isGroundPass && ball.MoveType == EBallMoveType.GroundPass)
            {
                Gizmos.color = Color.blue;
                //the pos ball can get in half time
                Vector3D interceptCenterPos = (3 * ball.OriginalPos + 5 * ball.TargetPos) / 8d;
                interceptCenterPos.Y = 0;
                
                double interceptInitRadius = attackTeam.PlayerList[0].BaseVelocity * interceptRunRate * (ball.FlyingTime / 2f - 0.2d);

                Gizmos.DrawWireSphere(new Vector3((float)interceptCenterPos.X,
                                                  (float)interceptCenterPos.Y,
                                                  (float)interceptCenterPos.Z),
                                                  (float)interceptInitRadius);
            }
        }

        //lines
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(new Vector3((float)defendTeam.Goal.GoalPos.X,
                                          (float)defendTeam.Goal.GoalPos.Y,
                                          (float)defendTeam.Goal.GoalPos.Z),
                                            (float)headShootLimit);

        //green for ballcontroller
        if (attackTeam.BallController != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(new Vector3((float)attackTeam.BallController.GetPosition().X,
                                           (float)attackTeam.BallController.GetPosition().Y + 2,
                                           (float)attackTeam.BallController.GetPosition().Z), 0.25f);

            Gizmos.DrawCube(new Vector3 ((float)attackTeam.BallController.TargetPos.X,
                                           (float)attackTeam.BallController.TargetPos.Y,
                                           (float)attackTeam.BallController.TargetPos.Z), 
                            new Vector3 (0.5f,0.5f,0.5f));

            Gizmos.DrawWireSphere(new Vector3 ((float)attackTeam.BallController.GetPosition().X,
                                               (float)attackTeam.BallController.GetPosition().Y,
                                               (float)attackTeam.BallController.GetPosition().Z),
                                  (float)shortPassLimit);
        }
        
        //red for attack-supporters
        for (int i = 0; i<attackTeam.PlayerList.Count; i++)
        {
            if(attackTeam.PlayerList[i].State == EPlayerState.AttackSupport || 
               attackTeam.PlayerList[i].State == EPlayerState.ToAttackSupport)
            {
                if(NeedAttackSupport)
                {
                    Gizmos.color = Color.red;
                    Gizmos.DrawSphere(new Vector3((float)attackTeam.PlayerList[i].GetPosition().X,
                                                  (float)attackTeam.PlayerList[i].GetPosition().Y + 2,
                                                  (float)attackTeam.PlayerList[i].GetPosition().Z), 0.25f);
                    
                    Gizmos.DrawCube(new Vector3 ((float)attackTeam.PlayerList[i].TargetPos.X,
                                                 (float)attackTeam.PlayerList[i].TargetPos.Y,
                                                 (float)attackTeam.PlayerList[i].TargetPos.Z),
                                    new Vector3 (0.75f,0.25f,0.75f));

                    Gizmos.DrawLine(new Vector3 ((float)attackTeam.PlayerList[i].TargetPos.X,
                                                 (float)attackTeam.PlayerList[i].TargetPos.Y,
                                                 (float)attackTeam.PlayerList[i].TargetPos.Z),
                                    new Vector3((float)attackTeam.BallController.GetPosition().X,
                                                (float)attackTeam.BallController.GetPosition().Y,
                                                (float)attackTeam.BallController.GetPosition().Z));

                    Gizmos.DrawWireSphere(new Vector3((float)attackTeam.PlayerList[i].TargetPos.X,
                                                      (float)attackTeam.PlayerList[i].TargetPos.Y,
                                                      (float)attackTeam.PlayerList[i].TargetPos.Z),4f);
                }
            }
            else if(attackTeam.PlayerList[i].State == EPlayerState.HomePos)
            {
                if(NeedAttackerHomePos)
                {
                    Gizmos.color = Color.white;
                    Gizmos.DrawCube(new Vector3 ((float)attackTeam.PlayerList[i].HomePos.X,
                                                 (float)attackTeam.PlayerList[i].HomePos.Y,
                                                 (float)attackTeam.PlayerList[i].HomePos.Z),
                                    new Vector3 (0.75f,0.25f,0.75f));
                    
                    Gizmos.DrawLine(new Vector3 ((float)attackTeam.PlayerList[i].HomePos.X,
                                                 (float)attackTeam.PlayerList[i].HomePos.Y,
                                                 (float)attackTeam.PlayerList[i].HomePos.Z),
                                    new Vector3((float)attackTeam.PlayerList[i].GetPosition().X,
                                                (float)attackTeam.PlayerList[i].GetPosition().Y,
                                                (float)attackTeam.PlayerList[i].GetPosition().Z));
                }
            }
        }

        for (int i = 0; i<defendTeam.PlayerList.Count; i++)
        {
            if(defendTeam.PlayerList[i].State == EPlayerState.Intercept || 
               defendTeam.PlayerList[i].State == EPlayerState.Intercept_Failed || 
               defendTeam.PlayerList[i].State == EPlayerState.Intercept_Success)
            {
                if(NeedInterceptRange)
                {
                    Gizmos.color = Color.blue;
                    Gizmos.DrawSphere(new Vector3((float)defendTeam.PlayerList[i].GetPosition().X,
                                                  (float)defendTeam.PlayerList[i].GetPosition().Y + 2,
                                                  (float)defendTeam.PlayerList[i].GetPosition().Z), 0.25f);
                    
                    Gizmos.DrawCube(new Vector3 ((float)defendTeam.PlayerList[i].TargetPos.X,
                                                 (float)defendTeam.PlayerList[i].TargetPos.Y,
                                                 (float)defendTeam.PlayerList[i].TargetPos.Z),
                                    new Vector3 (0.75f,0.25f,0.75f));
                    
                    Gizmos.DrawLine(new Vector3 ((float)defendTeam.PlayerList[i].TargetPos.X,
                                                 (float)defendTeam.PlayerList[i].TargetPos.Y,
                                                 (float)defendTeam.PlayerList[i].TargetPos.Z),
                                    new Vector3 ((float)defendTeam.PlayerList[i].GetPosition().X,
                                                 (float)defendTeam.PlayerList[i].GetPosition().Y,
                                                 (float)defendTeam.PlayerList[i].GetPosition().Z));
                }
            }
            //orange for markwithball
            if(defendTeam.PlayerList[i].MarkingStatus == Common.EMarkStatus.MARKWITHBALL)
            {
                if(NeedMarkWithBallTrack)
                {
                    Gizmos.color = new Color(1f,97f/255f,0f);
                    Gizmos.DrawSphere(new Vector3((float)defendTeam.PlayerList[i].GetPosition().X,
                                                   (float)defendTeam.PlayerList[i].GetPosition().Y + 2,
                                                   (float)defendTeam.PlayerList[i].GetPosition().Z), 0.25f);
                    
                    Gizmos.DrawCube(new Vector3 ((float)defendTeam.PlayerList[i].TargetPos.X,
                                                   (float)defendTeam.PlayerList[i].TargetPos.Y,
                                                   (float)defendTeam.PlayerList[i].TargetPos.Z),
                                    new Vector3 (0.75f,0.25f,0.75f));
                }
            }
            else if(defendTeam.PlayerList[i].MarkingStatus == Common.EMarkStatus.MARKWITHOUTBALL)
            {   
                if(NeedMarkWithoutBallTrack)
                {
                    //yellow for others
                    Gizmos.color = Color.yellow;
                    Gizmos.DrawSphere(new Vector3((float)defendTeam.PlayerList[i].GetPosition().X,
                                                   (float)defendTeam.PlayerList[i].GetPosition().Y + 2,
                                                   (float)defendTeam.PlayerList[i].GetPosition().Z), 0.25f);
                    
                    Gizmos.DrawSphere(new Vector3 ((float)defendTeam.PlayerList[i].TargetPos.X,
                                                   (float)defendTeam.PlayerList[i].TargetPos.Y,
                                                   (float)defendTeam.PlayerList[i].TargetPos.Z),0.15f);
    //                Gizmos.DrawCube(new Vector3 ((float)defendTeam.PlayerList[i].TargetPos.X,
    //                                               (float)defendTeam.PlayerList[i].TargetPos.Y,
    //                                               (float)defendTeam.PlayerList[i].TargetPos.Z),
    //                                new Vector3 (0.5f,1.5f,0.5f));

                    //draw region for opponent
                    Vector3 opponentPos = new Vector3((float)defendTeam.PlayerList[i].Opponent.GetPosition().X,
                                                      (float)defendTeam.PlayerList[i].Opponent.GetPosition().Y,
                                                      (float)defendTeam.PlayerList[i].Opponent.GetPosition().Z);
                    Vector3 defenderGoalPos = new Vector3((float)defendTeam.Goal.GoalPos.X,
                                                          (float)defendTeam.Goal.GoalPos.Y,
                                                          (float)defendTeam.Goal.GoalPos.Z);
                    Vector3 LeftCorner = new Vector3((float)defendTeam.Goal.LeftCorner.X,
                                                     (float)defendTeam.Goal.LeftCorner.Y,
                                                     (float)defendTeam.Goal.LeftCorner.Z);
                    Vector3 RightCorner = new Vector3((float)defendTeam.Goal.RightCorner.X,
                                                      (float)defendTeam.Goal.RightCorner.Y,
                                                      (float)defendTeam.Goal.RightCorner.Z);
                    Vector3D LeftDir3D = MathUtil.GetDir(defendTeam.PlayerList[i].Opponent.GetPosition(), defendTeam.Goal.LeftCorner);
                    Vector3D RightDir3D = MathUtil.GetDir(defendTeam.PlayerList[i].Opponent.GetPosition(), defendTeam.Goal.RightCorner);

                    double goalAngleCenter = MathUtil.GetAngle(defendTeam.PlayerList[i].Opponent.GetPosition(), defendTeam.Goal.GoalPos);
                    double goalAngleLeft = MathUtil.GetAngle(defendTeam.PlayerList[i].Opponent.GetPosition(), defendTeam.Goal.LeftCorner);
                    double goalAngleRight = MathUtil.GetAngle(defendTeam.PlayerList[i].Opponent.GetPosition(), defendTeam.Goal.RightCorner);                
                    if (MathUtil.GetMinAngle(goalAngleLeft, goalAngleCenter) < 30)
                    {
                        //change to a min dir with a delta-Angle 30
                        LeftDir3D = MathUtil.GetDirFormAngle(goalAngleCenter-30);
                    }
                    if (MathUtil.GetMinAngle(goalAngleRight, goalAngleCenter) < 30)
                    {
                        //change to a min dir with a delta-Angle 30
                        RightDir3D = MathUtil.GetDirFormAngle(goalAngleCenter+30);
                    }

                    Vector3 LeftDir = new Vector3((float)LeftDir3D.X,
                                                  (float)LeftDir3D.Y,
                                                  (float)LeftDir3D.Z);
                    Vector3 RightDir = new Vector3((float)RightDir3D.X,
                                                   (float)RightDir3D.Y,
                                                   (float)RightDir3D.Z);                
    //                Gizmos.DrawLine(opponentPos,LeftCorner);
    //                Gizmos.DrawLine(opponentPos,RightCorner);
                    Gizmos.DrawLine(opponentPos,defenderGoalPos);
                    Gizmos.DrawLine(opponentPos,opponentPos + 6f * LeftDir);
                    Gizmos.DrawLine(opponentPos,opponentPos + 6f * RightDir);
                    Gizmos.DrawLine(opponentPos + 6f * LeftDir,
                                    opponentPos + 6f * RightDir);
                }
            }
            else if(defendTeam.PlayerList[i].State == EPlayerState.HomePos)
            {
                if(NeedDefenderHomePos)
                {
                    Gizmos.color = Color.white;
                    Gizmos.DrawCube(new Vector3 ((float)defendTeam.PlayerList[i].HomePos.X,
                                                 (float)defendTeam.PlayerList[i].HomePos.Y,
                                                 (float)defendTeam.PlayerList[i].HomePos.Z),
                                    new Vector3 (0.75f,0.25f,0.75f));
                    
                    Gizmos.DrawLine(new Vector3 ((float)defendTeam.PlayerList[i].HomePos.X,
                                                 (float)defendTeam.PlayerList[i].HomePos.Y,
                                                 (float)defendTeam.PlayerList[i].HomePos.Z),
                                    new Vector3((float)defendTeam.PlayerList[i].GetPosition().X,
                                                (float)defendTeam.PlayerList[i].GetPosition().Y,
                                                (float)defendTeam.PlayerList[i].GetPosition().Z));
                }
            }
        }
    }
    #endif
}
