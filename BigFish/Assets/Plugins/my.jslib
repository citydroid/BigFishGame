mergeInto(LibraryManager.library, {

  Hello: function () {
    window.alert("Hello, world!");
	console.log("Hello, world!");
  },

  HelloString: function (str) {
    window.alert(UTF8ToString(str));
  },
  
  GiveMePlayerData: function() {
	console.log(player.getName());
	SendMessage('Yandex','SetName', player.getName());
  },
  
  
  
  GetLeader: function(){
	  ysdk.getLeaderboards()
	  .then(lb => lb.getLeaderboardDescription('leaderboard2021'))
	  .then(res => console.log(res));
  },
  
  
  
  ShowAdv: function(){
	ysdk.adv.showFullscreenAdv({
		callbacks: {
			onClose: function(wasShown) {
				window.alert("------closed-------");
				  // Действие после закрытия рекламы.
			},
			onError: function(error) {
				  // Действие в случае ошибки.
			}
		}
	})
  },
  
  RewardedAdv: function(){
	ysdk.adv.showRewardedVideo({
		callbacks: {
			onOpen: () => {
			  console.log('Video ad open.');
			},
			onRewarded: () => {
			  console.log('Rewarded!');
			  SendMessage('Yandex','AfterAdvAction');
			},
			onClose: () => {
			  console.log('Video ad closed.');
			},
			onError: (e) => {
			  console.log('Error while open video ad:', e);
			}
		}
	})
  },


  SaveExtern: function(date){
	  var dateString = UTF8ToString(date);
	  var myobj = JSON.parse(dateString);
	  player.setData(myobj);
  },  
  
  LoadExtern: function(){
	  player.getData().then(_date => {
		  const myJSON = JSON.stringify(_date);
		  SendMessage('Progress', 'SetPlayerInfo', myJSON);
	  });
  },  
  
});