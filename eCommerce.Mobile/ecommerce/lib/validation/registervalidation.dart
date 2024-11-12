
import 'package:flutter/material.dart';

class Registervalidation {

  static RegExp passwordRegExp = RegExp(r'^(?=.*[A-Za-z])(?=.*\d)(?=.*[@$!%*#?&])[A-Za-z\d@$!%*#?&]{8,}$');
  static RegExp phoneRegExp = RegExp(r'^((\+91-?)|0)?[0-9]{10}$'); 
  static RegExp emailRegExp = RegExp(r'^[a-z0-9._%+-]+@[a-z0-9.-]+\.[a-z]{2,4}$');
  static RegExp pincodeRegExp = RegExp(r'^[1-9]{1}[0-9]{2}\s{0,1}[0-9]{3}$');

  static String firstName='';
  static bool firstNameError = false;
  static bool firstNameTouch = false;
  static FocusNode firstNameFocusNode = FocusNode();
  static final TextEditingController firstNameController = TextEditingController();

  static String lastName='';
  static FocusNode lastNameFocusNode = FocusNode();
  static final TextEditingController lastNameController = TextEditingController();

  static String mobileNo='';
  static bool mobileNoError = false;
  static bool mobileNoTouch = false;
  static FocusNode mobileNoFocusNode = FocusNode();
  static final TextEditingController mobileNoController = TextEditingController();

  static String password='';
  static bool passwordError = false;
  static bool passwordTouch = false;
  static FocusNode passwordFocusNode = FocusNode();
  static final TextEditingController passwordController = TextEditingController();  

  static String eMail='';
  static FocusNode eMailFocusNode = FocusNode();
  static final TextEditingController eMailController = TextEditingController();

  static String pincode='';
  static bool pincodeError = false;
  static bool pincodeTouch = false;
  static FocusNode pincodeFocusNode = FocusNode();
  static final TextEditingController pincodeController = TextEditingController();

  static String deliveryAddress='';
  static FocusNode deliveryAddressFocusNode = FocusNode();
  static final TextEditingController deliveryAddressController = TextEditingController();

  static String storeName='';
  static bool storeNameError = false;
  static bool storeNameTouch = false;
  static FocusNode storeNameFocusNode = FocusNode();
  static final TextEditingController storeNameController = TextEditingController();

  static String deliveryPincode='';
  static bool deliveryPincodeError = false;
  static bool deliveryPincodeTouch = false;
  static FocusNode deliveryPincodeFocusNode = FocusNode();
  static final TextEditingController deliveryPincodeController = TextEditingController();
  static List<String> deliveryCodes = [];  
}