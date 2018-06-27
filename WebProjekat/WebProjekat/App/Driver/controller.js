angular.module('driver.controllers', [])
.controller('driverController', ['$http', '$scope', '$rootScope', '$window', '$state', 'driverService',
	function ($http, $scope, $rootScope, $window, $state, driverService) {
		$scope.current = {};
		$scope.activeRides = [];
		driverService.getRides().then(function (response) {
			var rides = response.data;
			rides.forEach(ride => {
				if (ride.status >= 1 && ride.status <= 3 && ride.driver.id == $rootScope.user.id) {
					$scope.current = ride;
				} 
			})
		});

		driverService.search({}).then(function (response) {
			var rides = response.data;
			rides.forEach(ride => {
				if (ride.status < 3) {
					$scope.activeRides.push(ride);
				}
			})
		});
		
		$scope.setForUpdate = function (ride) {
			$scope.updateRide = ride;
		}

		$scope.update = function (ride) {
			console.log(ride);
			console.log($scope.updateRide);
			if (ride.status == 5) {
				driverService.updateRide(ride).then(function (response) {
					$window.location.reload();
				});
			} else if (ride.status == 6) {
				driverService.updateFailedRide(ride.id, ride.comment.description).then(function (response) {
					$window.location.reload();
				});
			}
		}

		$scope.accept = function(ride) {
			ride.assignDriverId = $rootScope.user.id;
			ride.newStatus = 3;
			driverService.accept(ride).then(function(response) {
				var resp = response.data;
				if(resp.isSuccess) {
					$window.location.reload();
				} else {
					alert(resp.message);
				}
			})
		}

	}])

.controller('driverHistoryController', ['$http', '$scope', '$rootScope', '$window', '$state', 'driverService',
	function ($http, $scope, $rootScope, $window, $state, driverService) {
		$scope.search = {};
		$scope.refreshList = function () {
			driverService.search($scope.search).then(function (response) {
				$scope.history = response.data;
			});
		}

		$scope.reset = function () {
			$scope.search = {};
			$scope.refreshList();
		}

		$scope.setForUpdate = function (ride) {
			$scope.updateRide = ride;
		}

		$scope.update = function (ride) {
			if (ride.status == 5) {
				driverService.updateRide(ride).then(function (response) {
					var resp = response.data;
					if(resp.isSuccess) {
						$window.location.reload();
					} else {
						alert(resp.message);
					}
				});
			} else if (ride.status == 6) {
				driverService.updateFailedRide(ride.id, ride.comment.description).then(function (response) {
					var resp = response.data;
					if(resp.isSuccess) {
						$window.location.reload();
					} else {
						alert(resp.message);
					}
				});
			}
		}
		
		$scope.accept = function(ride) {
			ride.assignDriverId = $rootScope.user.id;
			ride.newStatus = 3;
			driverService.accept(ride).then(function(response) {
				var resp = response.data;
				if(resp.isSuccess) {
					$window.location.reload();
				} else {
					alert(resp.message);
				}
			})
		}

		$scope.setComment = function(comment) {
			$scope.checkoutComment = comment;
		}
		
		$scope.refreshList();
	}])

.controller('driverLocationController', ['$http', '$scope', '$rootScope', '$window', '$state', 'driverService', 'profileService',
	function ($http, $scope, $rootScope, $window, $state, driverService, profileService) {
		profileService.getProfile().then(function(response) {
			var driver = response.data;
			$scope.location = driver.location;
		})

		$scope.updateLocation = function () {
			var loc = $scope.location;
			driverService.updateLocation(loc).then(function (response) {
				$window.location.reload();
			});
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
			$scope.location.x = pos[1];
			$scope.location.y = pos[0];
			
			$http.get('https://nominatim.openstreetmap.org/reverse?format=json&lon=' + pos[0] + '&lat=' + pos[1])
				.then(function(result) {
					$scope.location.streetName = result.data.address.road;
					$scope.location.streetNumber = parseInt(result.data.address.house_number);
					$scope.location.areaCode = result.data.address.postcode;
				});
			
		})
		draw = new ol.interaction.Draw({
			source: source,
			type: 'Point'
		});

		map.addInteraction(draw);
	}])