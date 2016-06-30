using System;
using Common;


public class CreatePlayerMessage : Message
{
    public CreatePlayerMessage(LLUnit kPlayer, PlayerInfo kInfo, bool bGoalKeeper)
        : base(MessageType.CreatePlayer)
    {
        Unit = kPlayer;
        IsGoalKeeper = bGoalKeeper;
        PlayerInfo = kInfo;
    }

    public readonly LLUnit Unit= null;
    public readonly bool IsGoalKeeper = false;
    public readonly PlayerInfo PlayerInfo;
}

public class RemovePlayerMessage : Message
{
    public RemovePlayerMessage(ETeamColor kType, LLUnit kPlayer, bool bGoalKeeper)
        : base(MessageType.RemovePlayer)
    {
        m_kTeamColor = kType;
        m_kPlayer = kPlayer;
        m_bGoalKeeper = bGoalKeeper;
    }

    public LLUnit Unit
    {
        get { return m_kPlayer; }
        
    }

    public ETeamColor TeamColor
    {
        get { return m_kTeamColor; }
        
    }

    public bool IsGoalKeeper
    {
        get { return m_bGoalKeeper; }
        
    }

    private LLUnit m_kPlayer = null;
    private ETeamColor m_kTeamColor;
    private bool m_bGoalKeeper = false;
}

public class ChangePlayerStateMessage : Message
{
    public ChangePlayerStateMessage(LLUnit kPlayer,EPlayerState kState)
        : base(MessageType.ChangePlayerState)
    {
        Player = kPlayer;
        State = kState;
    }

    public readonly LLUnit Player = null;
    public readonly EPlayerState State = EPlayerState.PS_END;
}


public class SelectPlayerStateMessage:Message
{
    private ETeamColor m_kTeamColor;
    private LLUnit  m_kUint;
    private int m_iColor;

    public ETeamColor TeamColor
    {
        get{return m_kTeamColor;}
        set{m_kTeamColor = value;}
    }

    public LLUnit Uint
    {
        get{return m_kUint;}
        set{m_kUint = value;}
    }

    public int Color
    {
        get{return m_iColor;}
        set{m_iColor = value;}
    }

    public SelectPlayerStateMessage(ETeamColor _kType,LLUnit _player,int _color)
        : base(MessageType.MessageSelectPlayerState)
    {
        m_kUint = _player;
        m_kTeamColor = _kType;
        m_iColor = _color;
    }
}



