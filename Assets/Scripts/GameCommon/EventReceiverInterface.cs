using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

interface EventReceiverInterface
{
   void OnDoubleClick(GameObject who, string pram, System.Object userData);
   void OnClick(GameObject who, string pram, System.Object userData);
}
