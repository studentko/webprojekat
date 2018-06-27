angular.module('register.services', [])
.service('registerService', [ '$http', function($http) {
	var path = "/api/authentication";
    
    this.register = function(user) {
        return $http.post(path + "/register", user);
    }
}]) 