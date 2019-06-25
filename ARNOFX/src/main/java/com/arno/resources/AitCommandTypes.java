package com.arno.resources;

public enum AitCommandTypes {
    Wait,                   //Waiting...
    StopWaiting,            //Stop waitingS
    ClientInitConnection,   //Connect arno.server to arno.client/ send id to arno.client
    RejestractionRequest,   //arno.client is not reqister and must register self
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
