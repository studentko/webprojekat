angular.module('dispatcher.services', [])
.service('dispatcherService', [ '$http', function($http) {
	var path = "/api/user";
	
	this.getUsers = function() {
		return $http.get(path);
	}

	this.getDrivers = function(location) {
		return $http.post("/api/driver", location);
	}

	this.createDriver = function(driver) {
		return $http.post(path, driver);
	}

	this.createRide = function(ride) {
		return $http.post("/api/ride", ride);
	}

	this.changeBlockStatus = function(id, status) {
		return $http.put("api/user/put/"+ id +"/" + status);
	}

	this.search = function(search) {
		var params = Object.assign({}, search);
		if (params.fromOrderDate) {
			params.fromOrderDate = params.fromOrderDate.toISOString();
		}
		
		if (params.toOrderDate) {
			params.toOrderDate = params.toOrderDate.toISOString();
		} 
		
		return $http.get("/api/ride/?" + $.param(params));
	}

	this.updateRide = function(rideId, driverId) {
		return $http.put("/api/ride/" + rideId, driverId);
	}
}]) 