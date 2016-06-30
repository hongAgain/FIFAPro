namespace BehaviourTree
{

    /// <summary>
    /// BTAction is the base class for action nodes.
    /// It is where the actual gameplay logic happens.
    /// </summary>
    public class BTAction : BTNode 
    {

        public BTAction()
        {
            Name = "Action";
            DisplayName = "Action";
            NodeType = "BTAction";
        }


        sealed public override BTResult Tick(double fTime) 
        {
			BTResult tickResult = BTResult.Success;

			if (m_kStatus == BTActionStatus.Ready) 
            {
				Enter();
				m_kStatus = BTActionStatus.Running;
			}
			if (m_kStatus == BTActionStatus.Running) 
            {
                tickResult = Execute(fTime);
				if (tickResult != BTResult.Running) 
                {
					Exit();
					m_kStatus = BTActionStatus.Ready;
					IsRunnint = false;
				}
				else 
                {
					IsRunnint = true;
				}
			}
			return tickResult;
		}

		public override void Clear () 
        {
			base.Clear();

			if (m_kStatus != BTActionStatus.Ready) 
            {	// not cleared yet
				Exit();
				m_kStatus = BTActionStatus.Ready;
			}
		}

		/// <summary>
		/// Called when the action node is about to execute.
		/// </summary>
		protected virtual void Enter () 
        {
        }

		/// <summary>
		/// Called every frame if the action node is active.
		/// </summary>
        protected virtual BTResult Execute(double fTime) 
        {
            return BTResult.Failed;
        }

		/// <summary>
		/// Called when the action node finishes.
		/// </summary>
		protected virtual void Exit () 
        {
        }

		private enum BTActionStatus 
        {
			Ready,
			Running,
		}
        private BTActionStatus m_kStatus = BTActionStatus.Ready;
	}
}