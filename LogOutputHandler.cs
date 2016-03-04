using UnityEngine;
using System.Collections;

public class LogOutputHandler : MonoBehaviour {

    //Register the HandleLog function on scene start to fire on debug.log events
    public void OnEnable(){
				Application.logMessageReceived += HandleLog;
    }

    //Remove callback when object goes out of scope
		public void OnDisable(){
				Application.logMessageReceived -= HandleLog;
    }

    //Create a string to store log level in
    string level = "";

    //Capture debug.log output, send logs to Loggly
    public void HandleLog(string logString, string stackTrace, LogType type) {

        //Initialize WWWForm and store log level as a string
        level = type.ToString ();
        var loggingForm = new WWWForm();

        //Add log message to WWWForm
        loggingForm.AddField("LEVEL", level);
        loggingForm.AddField("Message", logString);
        loggingForm.AddField("Stack_Trace", stackTrace);

        //Add any User, Game, or Device MetaData that would be useful to finding issues later
        loggingForm.AddField("Device_Model", SystemInfo.deviceModel);
				StartCoroutine(SendData(loggingForm));
		}

		public IEnumerator SendData(WWWForm form){
        //Send WWW Form to Loggly, replace TOKEN with your unique ID from Loggly
        WWW sendLog = new WWW("http://logs-01.loggly.com/inputs/TOKEN/tag/Unity3D", form);
				yield return sendLog;
    }
}
