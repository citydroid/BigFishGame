mergeInto(LibraryManager.library,
{
	AuthorizationCheck: function (playerPhotoSize, scopes)
	{
		AuthorizationCheck(Pointer_stringify(playerPhotoSize), scopes);
	},
	
	OpenAuthDialog: function (playerPhotoSize, scopes)
	{
		OpenAuthDialog(Pointer_stringify(playerPhotoSize), scopes);
	},
	
	SaveYG: function (jsonData, flush)
	{
		SaveCloud(Pointer_stringify(jsonData), flush);
	},
	
	LoadYG: function ()
	{
		LoadCloud();
	},
	
	InitLeaderboard: function ()
	{
		InitLeaderboard();
	},
	
	SetLeaderboardScores: function (nameLB, score)
	{
		SetLeaderboardScores(Pointer_stringify(nameLB), score);
	},
	
	GetLeaderboardScores: function (nameLB, maxPlayers, quantityTop, quantityAround, photoSizeLB, auth)
	{
		GetLeaderboardScores(Pointer_stringify(nameLB), maxPlayers, quantityTop, quantityAround, Pointer_stringify(photoSizeLB), auth);
	},

	FullscreenShow: function ()
	{
		FullscreenShow();
	},

    RewardedShow: function (id)
	{
		RewardedShow(id);
	},
	
	LanguageRequest: function ()
	{
		LanguageRequest();
	},
	
	RequestingEnvironmentData: function()
	{
		RequestingEnvironmentData();
	},	

	Review: function()
	{
		Review();
	}	
});