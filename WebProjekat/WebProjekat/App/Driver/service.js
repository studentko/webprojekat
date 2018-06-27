angular.module('driver.services', [])
.service('driverService', [ '$http', function($http) {
	var path = "/api/driver";
	
	this.updateLocation = function(location) {
		return $http.put(path, location);
	}

	this.getRides = function() {
		return $http.get("/api/ride/?onlyAssigned=true");
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

	this.getAssigned = function(search) {
		return $http.get("/api/ride/?OnlyAssigned=true");
	}

	this.updateRide = function(ride) {
		console.log(ride);
		var requestData = { newStatus: ride.status, destinationLocation: ride.destination, amount: ride.amount};
		return $http.put("/api/ride/" + ride.id, requestData);
	}

	this.updateFailedRide = function(rideId, reason) {
		return $http.delete("/api/ride/" + rideId + "/" + reason);
	}

	this.accept = function(ride) {
		return $http.put("/api/ride/" + ride.id, ride);
	}
}]) 