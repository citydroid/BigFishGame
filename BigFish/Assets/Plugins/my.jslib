mergeInto(LibraryManager.library, {

  Hello: function () {
    window.alert("Hello, world!");
	console.log("Hello, world!");
  },

  HelloString: function (str) {
    window.alert(UTF8ToString(str));
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
  
  RewardedAdv: function(value){
	ysdk.adv.showRewardedVideo({
		callbacks: {
			onOpen: () => {
			  console.log('Video ad open.');
			},
			onRewarded: () => {
			  console.log('Rewarded!');
			  myGameInstance.SendMessage("PlayerGameOver","AdYandex",value);
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

});