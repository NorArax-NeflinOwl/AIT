package resources;

public enum AitCommandTypes {
    Wait,                   //Waiting...
    StopWaiting,            //Stop waitingS
    ClientInitConnection,   //Connect gui.server to gui.client/ send id to gui.client
    RejestractionRequest,   //gui.client is not reqister and must register self
    RejestractionSuc,       //Creating account success
    RejestractionInvalid,   //Creating account invalid
    RejestractionRespons,   //Creating account response
    VerifyAccData,          //Activation account
    Login,                  //Log in
    LoginSuc,               //Log in success information
    LoginInvalid,           //Log in invalid information
    Logout,                 //Log out
    LogoutSuc,              //Log out success information
    SendTo,                 //Send To
    SendFrom,               //Send From
    SendBct,                //Send broadcast
    ResetPass,              //Reset password
    ChangeData,             //Change account
    DeleteAcc               //Delete account
}
