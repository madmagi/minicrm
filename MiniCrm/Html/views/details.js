MyApp.details = function (params) {
    console.log(params);
    var result = ko.observable('');

    for (var i = 0; i < MyApp.Contacts.length; i++)
    {
       if (MyApp.Contacts[i].id === params.id)
        {
             result = MyApp.Contacts[i];
        }
    }
    

      // var data = MyApp.Contacts[0];  each one has a name,id
    var viewModel = {        
        id: result.id,
        name: result.name,
        phone: result.phone
    };

    console.log("ViewModel: ");
    console.log(viewModel);

    return viewModel;
};