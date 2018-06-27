angular.module('customer.services', [])
.service('customerService', [ '$http', function($http) {
	var path = "/api/ride";
	
	this.requestRide = function(ride) {
		var requestData = {location: ride.startLocation, carType: ride.carType};
		return $http.post(path, requestData);
	}

	this.getRides = function() {
		return $http.get(path);
	}

	this.cancel = function(ride) {
		return $http.delete(path + "/" + ride.ride.id + "/" + ride.reason);
	}

	this.update = function(ride) {
		var requestData = {location: ride.startLocation, carType: ride.carType};
		return $http.put(path + "/" + ride.id, requestData);
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

	this.comment = function(comment) {
		return $http.put("api/ride/"+ comment.rideId + "/comment", comment);
	}

}]) 