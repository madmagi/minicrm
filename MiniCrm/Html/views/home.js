MyApp.home = function (params) {

      // var data = MyApp.Contacts[0];  each one has a name,id
    var viewModel = {
        dataSource: MyApp.Contacts
    };    
    return viewModel;
};