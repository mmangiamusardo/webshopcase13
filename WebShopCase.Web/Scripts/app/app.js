(function () {
    var app = angular.module("wscApp", ['ngRoute', 'mobile-angular-ui', 'mobile-angular-ui.gestures', 'toaster']);
    
    app.run(function ($transform) {
        window.$transform = $transform;
    });

    app.run(['$rootScope', '$location', '$http', function ($rootScope, $location, $http) {

        $rootScope.$on('$locationChangeStart', function (event, next, current) {
            if (next.indexOf('?uiSidebarLeft') > 0) {
                event.preventDefault();
            }
        });

        
        $rootScope.$on('$routeChangeStart', function (e, curr, prev) {
            if (curr.$$route && curr.$$route.resolve) {
                // Show a loading message until promises are not resolved
                $rootScope.loading = true;
            }
        });
        

        $rootScope.$on('$routeChangeSuccess', function (e, curr, prev) {
            // Hide loading message
            $rootScope.loading = false;
        });

        //$http.defaults.transformRequest.push(function (data) {
        //    $rootScope.progress = true;
        //    return data;
        //});

        //$http.defaults.transformResponse.push(function (data) {
        //    $rootScope.progress = false;
        //    return data;
        //})

    }]); // end app.run

    app.config(['$routeProvider', function ($routeProvider) {

        $routeProvider.when('/article/page/:pageIndex/:pageSize', {
            templateUrl: 'Scripts/app/views/article.html',
            controller: 'ArticleCtrl',
            resolve: {
                articles: function (srvShop, $route) {
                    return srvShop.getArticles($route.current.params.pageIndex, $route.current.params.pageSize);
                }
            }
        });

        $routeProvider.when('/article', {
            templateUrl: 'Scripts/app/views/article.html',
            controller: 'ArticleCtrl',
            resolve: {
                articles: function (srvShop, $route) {
                    return srvShop.getArticles();
                }
            }
        });

        $routeProvider.when('/cart', {
            templateUrl: 'Scripts/app/views/cart.html',
            controller: 'CartCtrl'
        });

        $routeProvider.when('/checkout', {
            templateUrl: 'Scripts/app/views/checkout.html',
            controller: 'CheckoutCtrl'
        });

        $routeProvider.when('/process', {
            templateUrl: 'Scripts/app/views/process.html',
            controller: 'ProcessCtrl',
            resolve: {
                result: function (srvShop, srvSharedData) {
                    var res = srvShop.postOrder2(srvSharedData.getOrder());
                    return res;
                }
            }
        });

        $routeProvider.when('/process/:id', {
            templateUrl: 'Scripts/app/views/process.html',
            controller: 'ProcessCtrl',
            resolve: {
                order: function (srvShop, $route) {
                    return srvShop.getOrder($route.current.params.id);
                }
            }
        });

        $routeProvider.when('/register', {
            templateUrl: 'Scripts/app/views/register.html',
            controller: 'AccountCtrl'
        });

        $routeProvider.when('/signin', {
            templateUrl: 'Scripts/app/views/signin.html',
            controller: 'AccountCtrl'
        });

        $routeProvider.when('/profile', {
            templateUrl: 'Scripts/app/views/profile.html'
        });

        $routeProvider.when('/orders', {
            templateUrl: 'Scripts/app/views/orders.html'
        });

        $routeProvider.when('/wishes', {
            templateUrl: 'Scripts/app/views/wishes.html'
        });

        $routeProvider.otherwise({ redirectTo: '/article' });

    }]);

    app.constant('registerUrl', '/api/Account/Register');
    app.constant('tokenUrl', '/Token');
    app.constant('tokenKey', 'accessToken');

    app.controller('MainController', function ($scope, $log, $location, srvAccount) {

        $scope.isUserAuthenticated = function () {
            return srvAccount.isUserAuthenticated();
        }
        $scope.logout = function () {
            srvAccount.logout();
            $location.path('/');
        }
    });

    app.directive('modal', function () {
        return {
            restrict: 'E',
            templateUrl: 'Scripts/app/views/detail.html',
            link: function (scope, element, attrs) {
                scope.do = function () {
                    console.log('doing something...');
                }
            }
        };
    });

    app.directive('uiLadda', [function () {
        return {
            link: function (scope, element, attrs) {
                var Ladda = window.Ladda, ladda = Ladda.create(element[0]);

                scope.$watch(attrs.uiLadda,
                    function (newVal, oldVal) {
                        if (newVal) {
                            ladda.start();
                        }
                        else {
                            ladda.stop();
                        }
                    });
            }
        };
    }]);

}());