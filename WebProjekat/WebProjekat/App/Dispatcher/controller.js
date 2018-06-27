angular.module('dispatcher.controllers', [])
	.controller('dispatcherController', ['$http', '$scope', '$rootScope', '$window', '$state', 'dispatcherService',
		function ($http, $scope, $rootScope, $window, $state, dispatcherService) {
			$scope.search = {};
			$scope.refreshList = function () {
				dispatcherService.search($scope.search).then(function (response) {
					$scope.history = response.data;
				});
			}

			$scope.reset = function () {
				$scope.search = {};
				$scope.refreshList();
			}

			$scope.setComment = function(comment) {
				$scope.checkoutComment = comment;
			}
			
			dispatcherService.search({ "StatusFilter": 0 }).then(function (response) {
				$scope.activeRides = response.data;
				$scope.activeRides.forEach(function (ride) {
					dispatcherService.getDrivers(ride.startLocation).then(function (response) {
						ride.closestFive = response.data;
					})
				})
			});

			$scope.assign = function (ride) {
				driverId = { "AssignDriverId": ride.driver };
				dispatcherService.updateRide(ride.id, driverId).then(function () {
					$window.location.reload();
				})
			}

			$scope.refreshList();
		}])

	.controller('createDriverController', ['$http', '$scope', '$rootScope', '$window', '$state', 'dispatcherService',
		function ($http, $scope, $rootScope, $window, $state, dispatcherService) {
			$scope.create = function (driver) {
				dispatcherService.createDriver(driver).then(function (response) {
					var resp = response.data;
					if (resp.isSuccess) {
						$window.location.reload();
					} else {
						alert(resp.message);
					}
				});
			};
		}])

	.controller('dispatcherRideController', ['$http', '$scope', '$rootScope', '$window', '$state', 'dispatcherService',
		function ($http, $scope, $rootScope, $window, $state, dispatcherService) {
			$scope.ride = { location: {} };
			$scope.requestRide = function (ride) {
				dispatcherService.createRide(ride).then(function (response) {
					var resp = response.data;
					if (resp.isSuccess) {
						$window.location.reload();
					} else {
						alert(resp.message);
					}
				});
			};
			/*var intervalFunc = setInterval(function() {
				if($scope.ride.location.x && $scope.ride.location.y) {
					clearInterval(intervalFunc);
					dispatcherService.getDrivers(location).then(function (response) {
						$scope.closestFive = response.data;
					})
				}
			}, 500);*/

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
				$scope.ride.location.x = pos[1];
				$scope.ride.location.y = pos[0];
				
				dispatcherService.getDrivers($scope.ride.location).then(function (response) {
						$scope.closestFive = response.data;
					})
				
				$http.get('https://nominatim.openstreetmap.org/reverse?format=json&lon=' + pos[0] + '&lat=' + pos[1])
				.then(function(result) {
					$scope.ride.location.streetName = result.data.address.road;
					$scope.ride.location.streetNumber = parseInt(result.data.address.house_number);
					$scope.ride.location.areaCode = result.data.address.postcode;
				});
			})
			draw = new ol.interaction.Draw({
				source: source,
				type: 'Point'
			});

			map.addInteraction(draw);
		}])

	.controller('usersController', ['$http', '$scope', '$rootScope', '$window', '$state', 'dispatcherService',
		function ($http, $scope, $rootScope, $window, $state, dispatcherService) {
			$scope.changeBlockStatus = function (id, status) {
				dispatcherService.changeBlockStatus(id, status).then(function () {
					$window.location.reload();
				})
			}

			dispatcherService.getUsers().then(function (response) {
				$scope.users = response.data;
			})
		}])