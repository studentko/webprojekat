angular.module('profile.services', [])
.service('profileService', [ '$http', function($http) {
	var path = "/api/user";
	
	this.save = function(user) {
		return $http.put(path, user);
	}
	
	this.getProfile = function() {
		return $http.get("/api/user/1");
	}
}]) 