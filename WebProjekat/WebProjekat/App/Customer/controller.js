angular.module('customer.controllers', [])
.controller('customerController', ['$http','$scope', '$rootScope', '$window', '$state', 'customerService',
	function($http, $scope, $rootScope, $window, $state, customerService) {
		$scope.currentRide = {};
		$scope.forUpdate = false;
		$scope.ride = { startLocation : {} };
		customerService.getRides().then(function(response) {
			$scope.history = response.data;
			for(var i in $scope.history) {
				if($scope.history[i].status == 0) {
					$scope.currentRide = $scope.history[i];
					$scope.ride = $scope.currentRide;
					$scope.forUpdate = true;
					break; 
				}
			}
		})
		
		console.log($scope.ride);
		console.log($scope.currentRide);

        $scope.requestRide = function(ride) {
			
			var requestData = {location: ride.startLocation, carType: ride.carType};
			
			customerService.requestRide(ride).then(function(response) {
				var resp = response.data;
				if(resp.isSuccess) {
					$scope.currentRide = resp.ride;
					$scope.ride = resp.ride;
					$scope.forUpdate = true;
					
				} else {
					alert(resp.message);
				}
			})
		}

		$scope.update = function(ride) {
			/*$scope.currentRide.startLocation = ride.startLocation;
			$scope.currentRide.carType = ride.carType;*/
			console.log(ride);
			customerService.update(ride).then(function() {
				if(resp.isSuccess) {
					alert("Updated.");
				} else {
					alert(resp.message);
				}
			})
		}

		$scope.cancel = function(reason) {
			customerService.cancel({"ride" : $scope.currentRide, "reason" : reason}).then(function() {
				$window.location.reload();
			})
		}

		$scope.setForComment = function(ride) {
			$scope.rideForComment = ride;
		}

		$scope.commentRide = function(comment) {
			comment.rideId = $scope.rideForComment.id;
			customerService.comment(comment).then(function(response) {
				var resp = response.data;
				if(resp.isSuccess) {
					$window.location.reload();
				} else {
					alert(resp.message)
				}
			})
		}

		$scope.setComment = function(comment) {
			$scope.checkoutComment = comment;
		}

		$scope.refreshList = function() {
			customerService.search($scope.search).then(function(response) {
				$scope.history = response.data;
			});
		}

		$scope.reset = function() {
			$scope.search = {};
			$scope.refreshList();
		}

		var raster = new ol.layer.Tile({
			source: new ol.source.OSM()
		});
		var source = new ol.source.Vector({
			wrapX: false
		});
		var vector = new ol.layer.Vector({
			source: source
		});
		var view = new ol.View({ center: new ol.proj.fromLonLat([19.838650, 45.252512]), zoom: 14 });
		var map = new ol.Map({ target: 'map', layers: [raster, vector], view: view });

		map.on('click', function (event) {
			var pos = ol.proj.transform(event.coordinate, 'EPSG:3857', 'EPSG:4326');
			$scope.ride.startLocation.x = pos[1];
			$scope.ride.startLocation.y = pos[0];
			
			$http.get('https://nominatim.openstreetmap.org/reverse?format=json&lon=' + pos[0] + '&lat=' + pos[1])
				.then(function(result) {
					$scope.ride.startLocation.streetName = result.data.address.road;
					$scope.ride.startLocation.streetNumber = parseInt(result.data.address.house_number);
					$scope.ride.startLocation.areaCode = result.data.address.postcode;
				});
		})
		draw = new ol.interaction.Draw({
			source: source,
			type: 'Point'
		});

		map.addInteraction(draw);
}])