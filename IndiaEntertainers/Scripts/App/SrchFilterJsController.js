(function () {

    var SrchFilterJsController = function ($scope, $http, $log, $window, upload, $timeout, $filter) {
        $scope.SearchResult = [];
        $scope.ShowResult = false;
        $scope.ShowEntertainersFilter = true;
        $scope.Entertainers = [];
        $scope.FltrCategories = [];
        $scope.FltrCities = [];
        $scope.ShowCitiesFltrModel = false;
        $scope.ShowCategoriesFltrModel = false;
        $scope.FilterModel = {};
        $scope.FilterModel.FlCity = [];
        $scope.MyFlCity = [];
        $scope.FilterModel.FlCategory = [];
        $scope.FilterModel.FlbudgetMin = "";
        $scope.FilterModel.FlbudgetMax = "";
        $scope.FilterModel.FlGender = [];
        $scope.FilterModel.FlNationality = [];
        $scope.FilterModel.FlPerfLanguage = [];
        $scope.FilterModel.PageNo = 0;
        $scope.ShowLoadMoreOption = true;
        $scope.FirstCat = 0;

        //Main Search on Home Search by Entertainer and Category
        $scope.Search = function (Qury) {
            $scope.SrchMessage = "Please Wait..";
            if (Qury != null) {
                if (Qury.length >= 2) {
                    $http.get('/Home/Search?Qry=' + Qury).then(function (data) {
                        if (data.data == "Fail") {
                            $scope.UpProfileMessage = "An Error has occured while loading posts!";
                        } else {
                            $scope.SrchMessage = "";
                            $scope.SearchResult = data.data;
                            $scope.ShowResult = true;
                        }
                    })
            .catch(function () {
                $scope.error = "An Error has occured while loading posts!";
                $scope.loading = false;
            });
                }
            }
        }

        //AssignCatId
        $scope.Cat1checked = false;
        $scope.Cat2checked = false;
        $scope.Cat3checked = false;
        $scope.Cat4checked = false;
        $scope.Cat5checked = false;
        $scope.AssignCatId = function (id) {
            if (id == 15) {
                $scope.FilterModel.FlCategory[0] = id;
                $scope.Cat1checked = true;
            } else if (id == 9) {
                $scope.FilterModel.FlCategory[1] = id;
                $scope.Cat2checked = true;
            } else if (id == 11) {
                $scope.FilterModel.FlCategory[2] = id;
                $scope.Cat3checked = true;
            } else if (id == 5) {
                $scope.FilterModel.FlCategory[3] = id;
                $scope.Cat4checked = true;
            } else if (id == 2) {
                $scope.FilterModel.FlCategory[4] = id;
                $scope.Cat5checked = true;
            } else if (id == 12) {
                $scope.FilterModel.FlCategory[5] = id;
                $scope.Cat6checked = true;
            } else if (id == 14) {
                $scope.FilterModel.FlCategory[6] = id;
                $scope.Cat7checked = true;
            }
            $scope.FirstCat = id;
            $scope.InnerFilter();
        }

        //Filter by City and etc
        $scope.InnerFilter = function (loadmore) {
            if (loadmore == "Loadmore")
            {
                $scope.FilterModel.PageNo = $scope.FilterModel.PageNo + 1;
            }
            else {
                $scope.FilterModel.PageNo = 0;
                $scope.ShowLoadMoreOption = true;
            }
            $scope.Qury = "";
            $scope.ShowCitiesFltrModel = false;
            $scope.FilterModel.FlCity = $scope.MyFlCity;
            $scope.SrchFiltMessage = "Please Wait..";
            $http.post('/EntrCustom/InnerFilter', $scope.FilterModel).then(function (data) {
                if (data.data == "Fail") {
                    $scope.UpProfileMessage = "An Error has occured while loading posts!";
                } else {
                    $scope.SrchFiltMessage = "";
                    if ($scope.FilterModel.PageNo > 0)
                    {
                        if (data.data.length < 20) {
                            $scope.ShowLoadMoreOption = false;
                        }
                        $scope.Entertainers = $scope.Entertainers.concat(data.data);
                        $scope.ShowEntertainersFilter = true;
                    }
                    else {
                        if (data.data.length < 20) {
                            $scope.ShowLoadMoreOption = false;
                        }
                        $scope.Entertainers = data.data;
                        $scope.ShowEntertainersFilter = true;
                    }
                }
            })
    .catch(function () {
        $scope.error = "An Error has occured while loading posts!";
        $scope.loading = false;
    });
        }

        //GetCities
        $scope.GetCities = function (Qury) {
            $scope.SrchFiltcityMessage = "Please Wait..";
            if (Qury != null) {
                if (Qury.length >= 2) {
                    $http.get('/EntrCustom/GetCities?Qry=' + Qury).then(function (data) {
                        if (data.data == "Fail") {
                            $scope.ShowCitiesFltrModel = false;
                            $scope.SrchFiltcityMessage = "An Error has occured while loading posts!";
                        } else {
                            $scope.SrchFiltcityMessage = "";
                            $scope.FltrCities = data.data;
                            angular.forEach($scope.FltrCities, function (item) {
                                $scope.TempItem = $filter("filter")($scope.MyFlCity, { Id: item.Id });
                                if ($scope.TempItem.length > 0) {
                                    var index = $scope.FltrCities.indexOf($scope.TempItem);
                                    $scope.FltrCities.splice(index, 1);
                                }
                            });
                            $scope.ShowCitiesFltrModel = true;
                        }
                    })
            .catch(function () {
                $scope.error = "An Error has occured while loading posts!";
                $scope.loading = false;
            });
                }
                else { $scope.ShowCitiesFltrModel = false; }
            }
        }

        $scope.addCity = function (item) {
            $scope.TempItem = $filter("filter")($scope.MyFlCity, { Id: item.Id });
            if ($scope.TempItem.length > 0) { }
            else { $scope.MyFlCity.push(angular.copy(item)); }
        }

        $scope.deleteCity = function (item) {
            var index = $scope.MyFlCity.indexOf(item);
            $scope.MyFlCity.splice(index, 1);
        }

        //GetCategories
        $scope.GetCategories = function (Qury) {
            $scope.SrchFilcattMessage = "Please Wait..";
            if (Qury != null) {
                if (Qury.length >= 2) {
                    $http.get('/EntrCustom/GetCategories?Qry=' + Qury).then(function (data) {
                        if (data.data == "Fail") {
                            $scope.SrchFilcattMessage = "An Error has occured while loading posts!";
                            $scope.ShowCategoriesFltrModel = false;
                        } else {
                            $scope.SrchFilcattMessage = "";
                            $scope.FltrCategories = data.data;
                            $scope.ShowCategoriesFltrModel = true;
                        }
                    })
            .catch(function () {
                $scope.error = "An Error has occured while loading posts!";
                $scope.loading = false;
            });
                }
                else {
                    $scope.ShowCategoriesFltrModel = false;
                }
            }
        }

        //Reset Filter
        $scope.Resetfilters = function () {
            $scope.FilterModel = {};
            $scope.FilterModel.FlCity = [];
            $scope.FilterModel.FlCategory = [];
            $scope.FilterModel.FlbudgetMin = "";
            $scope.FilterModel.FlbudgetMax = "";
            $scope.FilterModel.FlGender = [];
            $scope.FilterModel.FlNationality = [];
            $scope.FilterModel.FlPerfLanguage = [];
            // $scope.ShowEntertainersFilter = false;
            $scope.MyFlCity = [];
            if ($scope.FirstCat == 15) {
                $scope.FilterModel.FlCategory[0] = $scope.FirstCat;
                $scope.Cat1checked = true;
            } else if ($scope.FirstCat == 9) {
                $scope.FilterModel.FlCategory[1] = $scope.FirstCat;
                $scope.Cat2checked = true;
            } else if ($scope.FirstCat == 11) {
                $scope.FilterModel.FlCategory[2] = $scope.FirstCat;
                $scope.Cat3checked = true;
            } else if ($scope.FirstCat == 5) {
                $scope.FilterModel.FlCategory[3] = $scope.FirstCat;
                $scope.Cat4checked = true;
            } else if ($scope.FirstCat == 2) {
                $scope.FilterModel.FlCategory[4] = $scope.FirstCat;
                $scope.Cat5checked = true;
            } else if ($scope.FirstCat == 12) {
                $scope.FilterModel.FlCategory[5] = $scope.FirstCat;
                $scope.Cat6checked = true;
            } else if ($scope.FirstCat == 14) {
                $scope.FilterModel.FlCategory[6] = $scope.FirstCat;
                $scope.Cat7checked = true;
            }
            $scope.InnerFilter();
        }
        
    };

    //Add controller to the app
    app.controller("SrchFilterJsController", SrchFilterJsController);
}());