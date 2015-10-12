angular.module("wscApp")
    .directive("articleDetail", function () {
        return {
            restrict: 'E',
            templateUrl: 'Scripts/app/components/articleCmp.html',
            controller: function ($scope, SharedState, srvArticle) {
                $scope.$watch(
                    function () {
                        return SharedState.get('event');
                    },

                    function (newValue) {
                        console.log('event changed to ' + newValue);
                        if (newValue) {
                            srvArticle.getArticle(newValue)
                                .success(function (data, status, headers, config) {
                                    $scope.article = data;
                                })
                                .error(function (result, status, headers, config) {
                                    console.log(result);
                                    toaster.pop({
                                        type: 'error',
                                        body: result,
                                        bodyOutputType: 'trustedHtml',
                                    })
                                });
                        }
                    });
                },
            link: function (scope, element, attrs) {
                scope.do = function () {
                    console.log('doing something...');
                };
            }
        }
    });