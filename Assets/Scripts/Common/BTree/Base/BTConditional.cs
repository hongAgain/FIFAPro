namespace BehaviourTree
{

    /// <summary>
    /// BTConditional is the base class for conditional nodes.
    /// It is usually used to check conditions.
    /// 
    /// Concrete conditional classes inheriting from this class should override the Check method.
    /// </summary>
    public abstract class BTConditional : BTNode 
    {
        public BTConditional()
        {
            Name = "Conditional";
            DisplayName = "Conditional";
            NodeType = "BTConditional";
        }

        sealed public override BTResult Tick(double fTime) 
        {
			if (Check())
            {
				return BTResult.Success;
			}
			else 
            {
				return BTResult.Failed;
			}
		}

		/// <summary>
		/// This is where the condition check happens.
		/// </summary>
		public virtual bool Check () 
        {
			return false;
		}
	}

}