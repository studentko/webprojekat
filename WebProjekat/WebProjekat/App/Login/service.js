angular.module('login.services', [])
	.service('loginService', ['$http', function ($http) {
		var path = "/api/authentication";

		this.isLoggedIn = function () {
			return $http.get(path + "/isLogged");
		}

		this.login = function (user) {
			return $http.post(path + "/login", user);
		}

		this.logout = function (user) {
			return $http.post(path + "/logout", {});
		}

		this.getProfile = function() {
			return $http.get("/api/user/1");
		}
	}]) 