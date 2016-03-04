#pragma strict

//Register the HandleLog function on scene start to fire on debug.log events
function OnEnable () {
	Application.logMessageReceived += HandleLog;
}

//Remove callback when object goes out of scope
function OnDisable () {
	Application.logMessageReceived -= HandleLog;
}

function HandleLog (logString : String, stackTrace : String, type : LogType) {
  //Initialize WWWForm and store log level as a string
  var level = type.ToString ();
  var loggingForm = new WWWForm();

  //Add log message to WWWForm
  loggingForm.AddField("LEVEL", level);
  loggingForm.AddField("Message", logString);
  loggingForm.AddField("Stack_Trace", stackTrace);

  //Add any User, Game, or Device MetaData that would be useful to finding issues later
  loggingForm.AddField("Device_Model", SystemInfo.deviceModel);
  SendStuff(loggingForm);
}
function SendStuff(form : WWWForm){
  //Send WWW Form to Loggly, replace TOKEN with your unique ID from Loggly
  var sendLog = new WWW("http://logs-01.loggly.com/inputs/TOKEN/tag/Unity3D", form);
  yield sendLog;
}
