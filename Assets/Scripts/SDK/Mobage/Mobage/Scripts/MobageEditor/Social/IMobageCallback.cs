
public interface IMobageCallback {
	
    void addLoginListenerComp(string message);

    void addLoginListenerRequied(string message);

    void addLoginListenerError(string message);

    void addLoginListenerCancel(string message);

	// Logout
    void LogoutComp(string message);

    void LogoutCancel(string message);
	
	// People
    void OnGetUserCompleteSuccess(string message);

    void OnGetUserCompleteError(string message);
	
    void OnGetUsersCompleteSuccess(string message);
	
    void OnGetUsersCompleteError(string message);
	// BlackList
    void OnCheckBlacklistCompleteSuccess(string message);
	
    void OnCheckBlacklistCompleteError(string message);
	
	// Service
	
    void OnDialogComplete(string message);
	
	// Auth
    void AuthorizeTokenSuccess(string message);
	
    void AuthorizeTokenError(string message);
	
	// Bank
	void TransactionWithDialogCompleteSuccess(string message);
	
	void TransactionWithDialogCompleteError(string message);
	
	void TransactionWithDialogCompleteCancel(string message);
		
	void TransactionCompleteSuccess(string message);
	
	void TransactionCompleteError(string message);
			
	void GetItemCompleteSuccess(string message);
	
	void GetItemCompleteError(string message);
			
	void  OnGetBalanceCompleteSuccess(string message);

	void  OnGetBalanceCompleteError(string message);
	
	void  GetvcNameStr(string message);
	
	void  GetMarketCode(string message);

	// Remote Notification
    void OnPushSendCompleteSuccess(string message);

    void OnPushSendCompleteError(string message);
	
    void OnPushGetEnableCompleteSuccess(string message);
	
    void OnPushGetEnableCompleteError(string message);

    void OnPushSetEnableCompleteSuccess(string message);

    void OnPushSetEnableCompleteError(string message);

    void OnPushHandleReceivedComplete(string message);
	
	//CNSocial
	void OnDashBoardComplete(string message);

	void OnGetMobageVendorIdComplete(string message);
}



