angular.module('profile.controllers', [])
.controller('profileController', ['$http','$scope', '$rootScope', '$window', '$state', 'profileService',
	function($http, $scope, $rootScope, $window, $state, profileService) {
		profileService.getProfile().then(function(response) {
			$rootScope.user = response.data;
		})
		$scope.save = function(user) {
			profileService.save(user).then(function(response) {
				$window.location.reload();
			})
		}
}])