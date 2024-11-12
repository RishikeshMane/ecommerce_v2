import 'package:flutter/material.dart';
import 'package:url_launcher/url_launcher.dart';

import 'package:ecommerce/utils/style.dart';
import 'package:ecommerce/validation/registervalidation.dart';
import 'package:ecommerce/utils/util.dart';
import 'package:ecommerce/services/register.service.dart';
import 'package:ecommerce/models/country.dart';
import 'package:ecommerce/models/state.dart';
import 'package:ecommerce/models/city.dart';
import 'package:ecommerce/models/userdetail.dart';

enum CustomerType { buyer, supplier }

class RegisterPage extends StatefulWidget {
  const RegisterPage({super.key, required this.title});

  final String title;

  @override
  State<RegisterPage> createState() => _RegisterPageState();
}

class _RegisterPageState extends State<RegisterPage> {

  ButtonStyle _currentButonStyle = const ButtonStyle();
  bool _hidePassword = true;
  bool _termsAndCondition = false;
  UserDetail _user = UserDetail();
    
  List<Country> _allCountries = [];
  List<States> _allStates = [];
  List<City> _allCities = [];

  List<String> _countries = [];
  String? _selectedCountry = '';
  List<String> _states = <String>[];
  String? _selectedState = '';
  List<String> _cities = <String>[];
  String? _selectedCity = '';
  CustomerType? _customerType = CustomerType.supplier; 

  @override
  void initState() {

    super.initState();

    updateCountries();

    Registervalidation.firstNameFocusNode.addListener((){
      firstNameNodeFocusListener();
    });

    Registervalidation.lastNameFocusNode.addListener((){
      lastNameNodeFocusListener();
    });

    Registervalidation.mobileNoFocusNode.addListener((){
      mobileNoNodeFocusListener();
    });

    Registervalidation.passwordFocusNode.addListener((){
      passwordNodeFocusListener();
    });

    Registervalidation.eMailFocusNode.addListener((){
      eMailNodeFocusListener();
    });

    Registervalidation.pincodeFocusNode.addListener((){
      pincodeNodeFocusListener();
    });

    Registervalidation.deliveryAddressFocusNode.addListener((){
      deliveryAddressNodeFocusListener();
    });

    Registervalidation.storeNameFocusNode.addListener((){
      storeNameNodeFocusListener();
    });

    Registervalidation.deliveryPincodeFocusNode.addListener((){
      deliveryPincodeFocusListener();
    });                                
  }

  @override
  void dispose(){
    super.dispose();
  }

  Future<void> updateCountries() async {

    if (_countries.isNotEmpty) return;
    
    RegisterService rs = RegisterService();
    await rs.getCountry();
    await rs.getState();
    await rs.getCity();
    try{
      setState((){
        _countries = rs.countries;
        _selectedCountry = 'India';
        _allCountries = rs.allCountries;

        _states = rs.states;
        _selectedState = 'Maharastra';
        _allStates = rs.allStates;

        _cities = rs.cities;
        _selectedCity = 'Mumbai';
        _allCities = rs.allCities;

        updateStates(_selectedCountry.toString());
        updateCities(_selectedCountry.toString(), _selectedState.toString());    
      });
    }
    catch(e){}
  }

  updateStates(String country)
  {
    _states = Util.getStates(country, _allCountries, _allStates);
    _selectedState = _states[0].toString();
  }

  updateCities(String country, String state)
  {
    int stateLinkId = Util.getStateLinkId(country, state, _allCountries, _allStates);
    _cities = Util.getCities(stateLinkId, country, _allCountries, _allCities);
    _selectedCity = _cities[0].toString();
  }

  void firstNameNodeFocusListener(){
    try{
      setState(() {
        Registervalidation.firstNameError = Registervalidation.firstNameController.text.isEmpty ? true : false;
        Registervalidation.firstNameTouch = true;
        Registervalidation.firstName = Registervalidation.firstNameController.text;
      });
    }
    catch(error)
    {}
  }

  void lastNameNodeFocusListener(){
    try{
      setState(() {
        Registervalidation.lastName = Registervalidation.lastNameController.text;
      });
    }
    catch(error)
    {}
  }

  void mobileNoNodeFocusListener(){
    try{
      setState(() {
        Registervalidation.mobileNo = Registervalidation.mobileNoController.text;
        Registervalidation.mobileNoError = Registervalidation.phoneRegExp.hasMatch(Registervalidation.mobileNo) ? false : true;
        Registervalidation.mobileNoTouch = true;
      });
    }
    catch(error)
    {}
  }

  void passwordNodeFocusListener(){
    try{
      setState(() {
        Registervalidation.password = Registervalidation.passwordController.text;
        Registervalidation.passwordError = Registervalidation.passwordRegExp.hasMatch(Registervalidation.password) ? false : true;
        Registervalidation.passwordTouch = true;
      });
    }
    catch(error)
    {}
  }  

  void eMailNodeFocusListener(){
    try{
      setState(() {
        Registervalidation.eMail = Registervalidation.eMailController.text;
      });
    }
    catch(error)
    {}
  }

  void pincodeNodeFocusListener(){
    try{
      setState(() {
        Registervalidation.pincode = Registervalidation.pincodeController.text;
        Registervalidation.pincodeError = Registervalidation.pincodeRegExp.hasMatch(Registervalidation.pincode) ? false : true;
        Registervalidation.pincodeTouch = true;
        if (Registervalidation.pincode.isNotEmpty && 
            !Registervalidation.pincodeError && 
            !Registervalidation.deliveryCodes.contains(Registervalidation.pincode))
        {
          Registervalidation.deliveryCodes.add(Registervalidation.pincode);
        }
      });
    }
    catch(error)
    {}
  }

  void deliveryAddressNodeFocusListener(){
    try{
      setState(() {
        Registervalidation.deliveryAddress = Registervalidation.deliveryAddressController.text;
      });
    }
    catch(error)
    {}
  }

  void storeNameNodeFocusListener(){
    try{
      setState(() {
        Registervalidation.storeNameError = Registervalidation.storeNameController.text.isEmpty ? true : false;
        Registervalidation.storeNameTouch = true;
        Registervalidation.storeName = Registervalidation.storeNameController.text;
      });
    }
    catch(error)
    {}
  }
        
  void deliveryPincodeFocusListener(){
    try{
      setState(() {
        Registervalidation.deliveryPincode = Registervalidation.deliveryPincodeController.text;
      });
    }
    catch(error)
    {}
  }  

  bool isRegistrable(){
    return (Registervalidation.firstNameTouch && !Registervalidation.firstNameError) && 
            (Registervalidation.mobileNoTouch && !Registervalidation.mobileNoError) &&
            (Registervalidation.passwordTouch && !Registervalidation.passwordError) &&
            (Registervalidation.pincodeTouch && !Registervalidation.pincodeError) &&
            ((Registervalidation.storeNameTouch && !Registervalidation.storeNameError) &&
            _termsAndCondition);
  }

  void register() async{
    RegisterService rs = RegisterService();
    updateUserDetail();

    String response = await rs.updateUsers(_user);

    if (response == 'success')
    {
      await _showRegisterDialog();
      Navigator.pop(context);
    }
    else if (response == 'duplicate number')
    {
      await _showAlreadyRegisterDialog();
    }
  }

  updateUserDetail(){
    _user.firstName = Registervalidation.firstName;
    _user.lastName = Registervalidation.lastName;
    _user.phone = Registervalidation.mobileNo;
    _user.email = Registervalidation.eMail;
    _user.password = Util.encrypt(Registervalidation.password);
    _user.country = _selectedCountry.toString();
    _user.state = _selectedState.toString();
    _user.city = _selectedCity.toString();
    _user.pincode = int.parse(Registervalidation.pincode);
    _user.address1 = Registervalidation.deliveryAddress;
    _user.store = Registervalidation.storeName;
    _user.subscribe = _termsAndCondition;
    _user.deliveryPinCodes = Registervalidation.deliveryCodes;
    _user.moreAddressCount = 0;
    _user.flagCode = Util.getFlagCode(_user.country, _allCountries); 
  }

  void enableDisableButton(){
    _currentButonStyle = isRegistrable() ? Style.enableButton : Style.disableButton;
  }

  validateControls(){
    enableDisableButton();
  }

  Future<void> _showRegisterDialog() async {
    return showDialog<void>(
      context: context,
      barrierDismissible: false, // user must tap button!
      builder: (BuildContext context) {
        return AlertDialog(
          title: const Text('Registration Successful !'),
          backgroundColor: const Color.fromRGBO(254, 254, 254, 1.0),
          content: const SingleChildScrollView(
            child: ListBody(
              children: <Widget>[
                Text('Please login and continue.',
                    style: TextStyle(
                    fontSize: 20,
                ),
                ),
              ],
            ),
          ),
          actions: <Widget>[
            TextButton(
              style: Style.enableButton,
              onPressed: () {
                Navigator.of(context).pop();
                ///Navigator.pop(context);
              },
              child: const Text('Ok'),
            ),
          ],
        );
      },
    );
  }

  Future<void> _showAlreadyRegisterDialog() async {
    return showDialog<void>(
      context: context,
      barrierDismissible: false, // user must tap button!
      builder: (BuildContext context) {
        return AlertDialog(
          title: const Text('Already Registered !'),
          backgroundColor: const Color.fromRGBO(254, 254, 254, 1.0),
          content: const SingleChildScrollView(
            child: ListBody(
              children: <Widget>[
                Text('This mobile no is already registered.',
                    style: TextStyle(
                    fontSize: 20,
                ),
                ),
              ],
            ),
          ),
          actions: <Widget>[
            TextButton(
              style: Style.enableButton,
              onPressed: () {
                Navigator.of(context).pop();
                ///Navigator.pop(context);
              },
              child: const Text('Ok'),
            ),
          ],
        );
      },
    );
  }

  @override
  Widget build(BuildContext context) {

    validateControls();
    updateCountries();

    return Scaffold(
      resizeToAvoidBottomInset: true,
      backgroundColor: Style.pageBackground,
      appBar: AppBar(
        title: Text(
          widget.title,
          style: const TextStyle(
            color: Color.fromRGBO(245, 245, 220, 1.0)
          ),
          ),
        centerTitle: true,
        elevation: 0.0,
        backgroundColor: const Color.fromRGBO(51, 155, 124, 1.0),
      ),
      body: SingleChildScrollView(
        physics: const BouncingScrollPhysics(parent: AlwaysScrollableScrollPhysics()),
        child: Column(
          crossAxisAlignment: CrossAxisAlignment.start,
          children: <Widget>[

            const SizedBox(
              height: 21.0,
            ),

            const Text(
              '( Registration on mobile device will be done as Supplier. If you want to register as Buyer visit our website at gogarden.co.in )',
              style: TextStyle(
                color: Color.fromRGBO(212, 115, 12, 1),
                letterSpacing: 1.0,
                fontWeight: FontWeight.bold,
              ),
            ),
            const SizedBox(
              height: 13.0,
            ),                        
            const Text(
              'First Name *',
              style: TextStyle(
                color: Color.fromRGBO(0, 128, 0, 1.0),
                letterSpacing: 1.0,
                fontWeight: FontWeight.bold,
              ),
            ),
            const SizedBox(
              height: 10.0,
            ),
            TextField(
              decoration: InputDecoration(
                filled: true,
                fillColor: Style.textBackground,                  
                border: OutlineInputBorder(
                  borderRadius: BorderRadius.circular(10.0),
                ),
                hintText: 'First Name',
              ),
              style: const TextStyle(
                height: 1.0,
              ),
                        
              controller: Registervalidation.firstNameController,
              focusNode: Registervalidation.firstNameFocusNode,    
            ),
            Visibility(
              visible: Registervalidation.firstNameError,
              child: const Text(
                'Name is required',
                style: TextStyle(
                  color: Color.fromRGBO(236, 44, 23, 1),
                  letterSpacing: 1.0,
                  fontWeight: FontWeight.bold,
                ),
              ),              
            ),            
            const SizedBox(
              height: 21.0,
            ),

            const Text(
              'Last Name',
              style: TextStyle(
                color: Color.fromRGBO(0, 128, 0, 1.0),
                letterSpacing: 1.0,
                fontWeight: FontWeight.bold,
              ),
            ),
            const SizedBox(
              height: 10.0,
            ),
            TextField(
              decoration: InputDecoration(
                filled: true,
                fillColor: Style.textBackground,                
                border: OutlineInputBorder(
                  borderRadius: BorderRadius.circular(10.0),
                ),
                hintText: 'Last Name',
              ),
              style: const TextStyle(
                height: 1.0,
              ),
              controller: Registervalidation.lastNameController,
              focusNode: Registervalidation.lastNameFocusNode,             
            ),
            const SizedBox(
              height: 21.0,
            ),

            const Text(
              'Mobile No *',
              style: TextStyle(
                color: Color.fromRGBO(0, 128, 0, 1.0),
                letterSpacing: 1.0,
                fontWeight: FontWeight.bold,
              ),
            ),
            const SizedBox(
              height: 10.0,
            ),
            TextField(
              decoration: InputDecoration(
                filled: true,
                fillColor: Style.textBackground,                
                border: OutlineInputBorder(
                  borderRadius: BorderRadius.circular(10.0),
                ),
                hintText: '9898989898',
              ),
              style: const TextStyle(
                height: 1.0,
              ),              
              controller: Registervalidation.mobileNoController,
              focusNode: Registervalidation.mobileNoFocusNode,    
            ),
            Visibility(
              visible: Registervalidation.mobileNoError,
              child: const Text(
                'Mobile No is invalid',
                style: TextStyle(
                  color: Color.fromRGBO(236, 44, 23, 1),
                  letterSpacing: 1.0,
                  fontWeight: FontWeight.bold,
                ),
              ),              
            ),            
            const SizedBox(
              height: 21.0,
            ),

            const Text(
              'Password *',
              style: TextStyle(
                color: Color.fromRGBO(0, 128, 0, 1.0),
                letterSpacing: 1.0,
                fontWeight: FontWeight.bold,
              ),
            ),
            const SizedBox(
              height: 10.0,
            ),
            TextField(
              decoration: InputDecoration(
                filled: true,
                fillColor: Style.textBackground,                
                border: OutlineInputBorder(
                  borderRadius: BorderRadius.circular(10.0),
                ),
                hintText: '********',
              ),
              style: const TextStyle(
                height: 1.0,
              ),
              obscuringCharacter: '*',
              obscureText: _hidePassword,       
              controller: Registervalidation.passwordController,
              focusNode: Registervalidation.passwordFocusNode,    
            ),
            Row(
              mainAxisAlignment: MainAxisAlignment.start,
              children: <Widget>[
                Checkbox(
                value: !_hidePassword,
                semanticLabel: 'ShowPassword',
                fillColor: WidgetStateProperty.resolveWith((states) {
                            if (!states.contains(WidgetState.selected)) {
                              return Style.textBackground;
                            }
                            return null;
                          }),                 
                onChanged: (show){
                  setState(() {
                    _hidePassword = !bool.parse(show.toString());
                  });
                },
              ),
              const Text(
              'Show Password',
              style: TextStyle(
                color: Color.fromRGBO(21, 21, 21, 1),
                letterSpacing: 1.0,
                fontWeight: FontWeight.w900,
              ),
              ),           
              ]
            ),
            Visibility(
              visible: Registervalidation.passwordError,
              child: const Text(
                'Password doesnot match criteria',
                style: TextStyle(
                  color: Color.fromRGBO(236, 44, 23, 1),
                  letterSpacing: 1.0,
                  fontWeight: FontWeight.bold,
                ),
              ),              
            ),
            const Text(
              'Minimum 8 characters and must contain alphabetic, numeric, one capital character and atleast one non alphanumeric characters',
              style: TextStyle(
                color: Color.fromRGBO(51, 51, 51, 1),
                letterSpacing: 1.0,
                fontWeight: FontWeight.bold,
              ),
            ),                        
            const SizedBox(
              height: 15.0,
            ),

            Row(
              mainAxisAlignment: MainAxisAlignment.start,
              children: <Widget>[              
                Expanded(
                  flex: 2,
                  child: Radio<CustomerType>(
                    value: CustomerType.buyer,
                    groupValue: _customerType,
                    onChanged: ((CustomerType? value){
                      setState(() {
                        _customerType = CustomerType.supplier;
                      });
                    }),
                  ),
                ),
                const Expanded(
                  flex: 10,
                  child: Text(
                          'I am Buyer',
                          style: TextStyle(
                            color: Color.fromRGBO(51, 51, 51, 1),
                            letterSpacing: 1.0,
                            fontWeight: FontWeight.bold,
                            fontSize: 15,
                          ),
                        ),
                ),

                Expanded(
                  flex: 2,
                  child: Radio<CustomerType>(
                    value: CustomerType.supplier,
                    groupValue: _customerType,
                    onChanged: ((CustomerType? value){
                      setState(() {
                        _customerType = value!;
                      });
                    }),
                  ),
                ),

                const Expanded(
                  flex: 10,
                  child: Text(
                          'I am Supplier',
                          style: TextStyle(
                            color: Color.fromRGBO(51, 51, 51, 1),
                            letterSpacing: 1.0,
                            fontWeight: FontWeight.bold,
                            fontSize: 15,
                          ),
                        ),
                ),                                      
              ]
            ),
            const SizedBox(
              height: 15.0,
            ),
            

            const Text(
              'Email Id',
              style: TextStyle(
                color: Color.fromRGBO(0, 128, 0, 1.0),
                letterSpacing: 1.0,
                fontWeight: FontWeight.bold,
              ),
            ),
            const SizedBox(
              height: 10.0,
            ),
            TextField(
              decoration: InputDecoration(
                filled: true,
                fillColor: Style.textBackground,                
                border: OutlineInputBorder(
                  borderRadius: BorderRadius.circular(10.0),
                ),
                hintText: 'name@domain.com',
              ),
              style: const TextStyle(
                height: 1.0,
              ),              
              controller: Registervalidation.eMailController,
              focusNode: Registervalidation.eMailFocusNode,    
            ),           
            const SizedBox(
              height: 21.0,
            ),

            Row(
              mainAxisAlignment: MainAxisAlignment.center,

              children: <Widget>[
                Expanded(
                  flex: 15,
                  child: Row(
                    mainAxisAlignment: MainAxisAlignment.start,
                    children: <Widget>[
                      const Text(
                        'Country    ',
                        style: TextStyle(
                          color: Color.fromRGBO(0, 128, 0, 1.0),
                          letterSpacing: 1.0,
                          fontWeight: FontWeight.bold,
                        ),
                      ),
                      Container(
                        padding: const EdgeInsets.symmetric(horizontal: 1, vertical: 1),
                        height: 35.0,
                        decoration: BoxDecoration(
                                      color: Colors.white, borderRadius: BorderRadius.circular(5),
                                      border: Border.all(
                                        color: const Color.fromRGBO(201, 201, 201, 1.0),
                                      ),
                                    ),
                        child: DropdownButton<String>(
                          value: _selectedCountry,
                          dropdownColor: const Color.fromRGBO(201, 201, 201, 1.0),
                          elevation: 10,
                          items: _countries.map<DropdownMenuItem<String>>((String value) {
                                    return DropdownMenuItem<String>(
                                      value: value,
                                      child: Text(value),
                                    );
                                  }).toList(),
                          onChanged: (String? selected){
                            setState(() {
                              _selectedCountry = selected;
                              updateStates(_selectedCountry.toString());
                              updateCities(_selectedCountry.toString(), _selectedState.toString());
                            });

                          },
                        ),
                      ),
                    
                    ]
                  ),
                ),

                Expanded(
                  flex: 15,
                  child: Row(
                    mainAxisAlignment: MainAxisAlignment.end,
                    children: <Widget>[
                      const Text(
                        'State    ',
                        style: TextStyle(
                          color: Color.fromRGBO(0, 128, 0, 1.0),
                          letterSpacing: 1.0,
                          fontWeight: FontWeight.bold,
                        ),
                      ),

                      Container(
                        padding: const EdgeInsets.symmetric(horizontal: 1, vertical: 1),
                        height: 35.0,
                        decoration: BoxDecoration(
                                      color: Colors.white, borderRadius: BorderRadius.circular(5),
                                      border: Border.all(
                                        color: const Color.fromRGBO(201, 201, 201, 1.0),
                                      ),
                                    ),
                        child: DropdownButton<String>(
                          value: _selectedState,
                          dropdownColor: const Color.fromRGBO(201, 201, 201, 1.0),
                          items: _states.map<DropdownMenuItem<String>>((String value) {
                                  return DropdownMenuItem<String>(
                                    value: value,
                                    child: Text(value),
                                  );
                                }).toList(),
                          onChanged: (String? selected){
                            setState(() {
                            _selectedState = selected;
                            updateCities(_selectedCountry.toString(), _selectedState.toString());
                            });
                          } ,
                        ),
                      ),
                  ]
                ),
                ),
              ]
            ),

            const SizedBox(
              height: 10.0,
            ),

            Row(
              mainAxisAlignment: MainAxisAlignment.center,

              children: <Widget>[
                Expanded(
                  flex: 15,
                  child: Row(
                    mainAxisAlignment: MainAxisAlignment.start,
                    children: <Widget>[
                      const Text(
                        'City          ',
                        style: TextStyle(
                          color: Color.fromRGBO(0, 128, 0, 1.0),
                          letterSpacing: 1.0,
                          fontWeight: FontWeight.bold,
                        ),
                      ),

                      Container(
                        padding: const EdgeInsets.symmetric(horizontal: 1, vertical: 1),
                        height: 35.0,
                        decoration: BoxDecoration(
                                      color: Colors.white, borderRadius: BorderRadius.circular(5),
                                      border: Border.all(
                                        color: const Color.fromRGBO(201, 201, 201, 1.0),
                                      ),
                                    ),
                        child: DropdownButton<String>(
                          value: _selectedCity,
                          dropdownColor: const Color.fromRGBO(201, 201, 201, 1.0),
                          elevation: 10,
                          items: _cities.map<DropdownMenuItem<String>>((String value) {
                                  return DropdownMenuItem<String>(
                                    value: value,
                                    child: Text(value),
                                  );
                                }).toList(),
                          onChanged: (String? selected){
                          setState(() {
                            _selectedCity = selected;
                            });
                        },
                        ),
                      ),

                    ]
                  ),
                ),
              ]
            ),

            const SizedBox(
              height: 10.0,
            ),

            const Text(
              'Pincode *',
              style: TextStyle(
                color: Color.fromRGBO(0, 128, 0, 1.0),
                letterSpacing: 1.0,
                fontWeight: FontWeight.bold,
              ),
            ),
            const SizedBox(
              height: 10.0,
            ),
            TextField(
              decoration: InputDecoration(
                filled: true,
                fillColor: Style.textBackground,                
                border: OutlineInputBorder(
                  borderRadius: BorderRadius.circular(10.0),
                ),
                hintText: '654321',
              ),
              style: const TextStyle(
                height: 1.0,
              ),
                        
              controller: Registervalidation.pincodeController,
              focusNode: Registervalidation.pincodeFocusNode,    
            ),
            Visibility(
              visible: Registervalidation.pincodeError,
              child: const Text(
                'Invalid pincode',
                style: TextStyle(
                  color: Color.fromRGBO(236, 44, 23, 1),
                  letterSpacing: 1.0,
                  fontWeight: FontWeight.bold,
                ),
              ),              
            ),            

            const SizedBox(
              height: 10.0,
            ),

            const Text(
              'Supplier Address',
              style: TextStyle(
                color: Color.fromRGBO(0, 128, 0, 1.0),
                letterSpacing: 1.0,
                fontWeight: FontWeight.bold,
              ),
            ),
            const SizedBox(
              height: 10.0,
            ),
            TextFormField (
              minLines: 2,
              maxLines: 2,
              decoration: InputDecoration(
                filled: true,
                fillColor: Style.textBackground,                
                border: OutlineInputBorder(
                  borderRadius: BorderRadius.circular(10.0),
                ),
              ),
              style: const TextStyle(
                height: 1.0,
              ),
                        
              controller: Registervalidation.deliveryAddressController,
              focusNode: Registervalidation.deliveryAddressFocusNode,    
            ),           

            const SizedBox(
              height: 10.0,
            ),

            const Text(
              'Storename *',
              style: TextStyle(
                color: Color.fromRGBO(0, 128, 0, 1.0),
                letterSpacing: 1.0,
                fontWeight: FontWeight.bold,
              ),
            ),
            const SizedBox(
              height: 10.0,
            ),
            TextField(
              decoration: InputDecoration(
                filled: true,
                fillColor: Style.textBackground,                
                border: OutlineInputBorder(
                  borderRadius: BorderRadius.circular(10.0),
                ),
              ),
              style: const TextStyle(
                height: 1.0,
              ),
                        
              controller: Registervalidation.storeNameController,
              focusNode: Registervalidation.storeNameFocusNode,
              onChanged: (String value){
                            setState(() {
                              Registervalidation.storeNameError = Registervalidation.storeNameController.text.isEmpty ? true : false;
                              Registervalidation.storeNameTouch = true;
                              Registervalidation.storeName = Registervalidation.storeNameController.text;                 
                          });},   
            ),
            Visibility(
              visible: Registervalidation.storeNameError,
              child: const Text(
                'Storename is required',
                style: TextStyle(
                  color: Color.fromRGBO(236, 44, 23, 1),
                  letterSpacing: 1.0,
                  fontWeight: FontWeight.bold,
                ),
              ),              
            ),

            const SizedBox(
              height: 25.0,
            ), 

            const Row(
              mainAxisAlignment: MainAxisAlignment.center,

              children: <Widget>[
                  Text(
                  '(Add Delivery Pincodes for products)',
                  style: TextStyle(
                    color: Color.fromRGBO(51, 155, 124, 1.0),
                    letterSpacing: 1.0,
                    fontWeight: FontWeight.bold,
                  ),
                ),                
              ],
            ),

            const SizedBox(
              height: 10.0,
            ), 
            const Row(
              mainAxisAlignment: MainAxisAlignment.center,

              children: <Widget>[
                  Text(
                  'Enter Delivery Pincode',
                  style: TextStyle(
                    color: Color.fromRGBO(51, 155, 124, 1.0),
                    letterSpacing: 1.0,
                    fontWeight: FontWeight.bold,
                  ),
                ),                
              ],
            ),            

            const SizedBox(
              height: 6.0,
            ),

            TextField(
              decoration: InputDecoration(
                filled: true,
                fillColor: Style.textBackground,                
                border: OutlineInputBorder(
                  borderRadius: BorderRadius.circular(10.0),
                ),
              ),
              style: const TextStyle(
              height: 1.0,
              ),
                    
              controller: Registervalidation.deliveryPincodeController,
              focusNode: Registervalidation.deliveryPincodeFocusNode,
              onChanged: (value){
                Registervalidation.deliveryPincode = value;
              } 
            ),
            Visibility(
              visible: Registervalidation.deliveryPincodeError,
              child: const Text(
                'Invalid pincode',
                style: TextStyle(
                  color: Color.fromRGBO(236, 44, 23, 1),
                  letterSpacing: 1.0,
                  fontWeight: FontWeight.bold,
                ),
              ),              
            ),            

            const SizedBox(
              height: 10.0,
            ),

            Row(
              mainAxisAlignment: MainAxisAlignment.center,

              children: <Widget>[
                Expanded(
                  flex: 10,
                  child: Row(
                    mainAxisAlignment: MainAxisAlignment.end,
                    children: <Widget>[

                      CircleAvatar(
                        backgroundColor: const Color.fromRGBO(51, 155, 124, 1.0),
                        radius: 23,
                        child: IconButton(
                          icon: const Icon(Icons.keyboard_double_arrow_down_rounded),
                          iconSize: 31,
                          color: const Color.fromRGBO(245, 245, 220, 1.0),
                          onPressed: () {
                            setState(() {
                              if (Registervalidation.deliveryPincode.isNotEmpty &&
                                  Registervalidation.pincodeRegExp.hasMatch(Registervalidation.deliveryPincode) &&
                                  !Registervalidation.deliveryCodes.contains(Registervalidation.deliveryPincode))
                              {
                                Registervalidation.deliveryPincodeError = false;
                                Registervalidation.deliveryCodes.add(Registervalidation.deliveryPincode);
                              }
                              else{
                                Registervalidation.deliveryPincodeError = true;
                              }                              
                            });                           
                          },
                        ),
                      ),    
                    ]
                  ),
                ),

                const Expanded(
                  flex: 2,
                  child: Row(
                    mainAxisAlignment: MainAxisAlignment.center,
                    children: <Widget>[
                      SizedBox(
                      width: 1.0,
                      ),
                    ]
                  ),
                ),                

                Expanded(
                  flex: 10,
                  child: Row(
                    mainAxisAlignment: MainAxisAlignment.start,
                    children: <Widget>[
                      CircleAvatar(
                        backgroundColor: const Color.fromRGBO(51, 155, 124, 1.0),
                        radius: 23,
                        child: IconButton(
                          icon: const Icon(Icons.keyboard_double_arrow_up_rounded),
                          iconSize: 31,
                          color: const Color.fromRGBO(245, 245, 220, 1.0),
                          onPressed: () {
                            setState(() {
                              if (Registervalidation.deliveryCodes.contains(Registervalidation.deliveryPincode))
                              {
                                Registervalidation.deliveryCodes.remove(Registervalidation.deliveryPincode);
                              }                               
                            });                           
                          },
                        ),
                      ),     
                    ]
                  ),
                ),
              ],
            ),

            const SizedBox(
              height: 6.0,
            ),

            Column(
              crossAxisAlignment: CrossAxisAlignment.stretch,
              children: Registervalidation.deliveryCodes.map((deliveryCode) => Text(
                  'Delivery Pincode $deliveryCode',
                  style: const TextStyle(
                    color: Color.fromRGBO(51, 155, 124, 1.0),
                    letterSpacing: 1.0,
                    fontWeight: FontWeight.bold,
                    fontSize: 16,
                  ),
                )
              ).toList(),
            ),

            const SizedBox(
              height: 10.0,
            ),

            Row(
              mainAxisAlignment: MainAxisAlignment.start,
              children: <Widget>[
                Checkbox(
                value: _termsAndCondition,
                fillColor: WidgetStateProperty.resolveWith((states) {
                            if (!states.contains(WidgetState.selected)) {
                              return Style.textBackground;
                            }
                            return null;
                          }),

                onChanged: (show){
                  setState(() {
                    _termsAndCondition = bool.parse(show.toString());
                  });
                },
              ),
              const Text(
              'I Accept with Terms and Condition',
              style: TextStyle(
                color: Color.fromRGBO(21, 21, 21, 1),
                letterSpacing: 1.0,
                fontWeight: FontWeight.w900,
              ),
              ),           
              ]
            ),

            const SizedBox(
              height: 10.0,
            ),

            Row(
              mainAxisAlignment: MainAxisAlignment.center,

              children: <Widget>[
                      Center(
                        child: ElevatedButton(
                        style: ButtonStyle(
                                backgroundColor: WidgetStateProperty.all<Color>(
                                  const Color.fromRGBO(245, 245, 220, 1.0)
                                ),
                                foregroundColor: WidgetStateProperty.all<Color>(
                                  const Color.fromRGBO(51, 155, 124, 1.0),
                                ),    
                              ),
                        onPressed: (){
                          try{
                            launchUrl(Uri.parse('https://www.google.com'));
                          }
                          catch(e){};
                        },
                        child: const Text(
                                  'gogarden.co.in',
                                  style: TextStyle(
                                    fontSize: 21,
                                  ),
                                ),
                    ),               
            ),

            ],
            ),

            const SizedBox(
              height: 10.0,
            ),                                   

            const SizedBox(
              height: 6.0,
            ),                         

            Center(child: ElevatedButton(
                      style: _currentButonStyle,
                      onPressed: !isRegistrable() ? null : () => register(),
                      child: const Text('  Register  '),
                    ),               
            ),
            const SizedBox(
              height: 21.0,
            ),
          ],
        ),
      ),
    );
  }
}
