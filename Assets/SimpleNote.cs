using UnityEngine;
using System.Collections;

namespace Softdrink{
	public class SimpleNote : MonoBehaviour {

		[SerializeField]
		[TextAreaAttribute]
		[TooltipAttribute("A note to display in the Inspector.")]
		private string note = "";

		public string getNote(){
			return note;
		}
	}
}
