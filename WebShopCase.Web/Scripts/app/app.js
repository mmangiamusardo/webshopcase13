(function () {
    var app = angular.module("wscApp", ['ngRoute', 'mobile-angular-ui', 'mobile-angular-ui.gestures', 'toaster']);
    
    app.constant('registerUrl', '/api/Account/Register');
    app.constant('tokenUrl', '/Token');
    app.constant('tokenKey', 'accessToken');
    app.constant('articleSrvUrl', 'http://localhost:50040/api/products/');
    app.constant('orderSrvUrl', 'http://localhost:50040/api/orders/');
    app.constant('articleSrvUrl', 'http://localhost:50040/api/products/');
    app.constant('profileSrvUrl', 'http://localhost:50040/api/customers/');

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


    }]); // end app.run

    app.config(['$routeProvider', function ($routeProvider) {

        $routeProvider.when('/article/page/:pageIndex/:pageSize', {
            templateUrl: 'Scripts/app/views/article.html',
            controller: 'ArticleCtrl',
            resolve: {
                articles: function (srvArticle, $route) {
                    return srvArticle.getArticles($route.current.params.pageIndex,
                        $route.current.params.pageSize);
                }
            }
        });

        $routeProvider.when('/article', {
            templateUrl: 'Scripts/app/views/article.html',
            controller: 'ArticleCtrl',
            resolve: {
                articles: function (srvArticle, $route) {
                    return srvArticle.getArticles();
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
                result: function (srvOrder, srvSharedData) {
                    var res = srvOrder.postOrder(srvSharedData.getOrder());
                    return res;
                }
            }
        });

        $routeProvider.when('/process/:id', {
            templateUrl: 'Scripts/app/views/process.html',
            controller: 'ProcessCtrl',
            resolve: {
                order: function (srvOrder, $route) {
                    return srvOrder.getOrder($route.current.params.id);
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

    app.controller('MainCtrl', function ($scope, $location, srvAccount) {

        $scope.isUserAuthenticated = function () {
            return srvAccount.isUserAuthenticated();
        }
        $scope.logout = function () {
            srvAccount.logout();
            $location.path('/');
        }
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