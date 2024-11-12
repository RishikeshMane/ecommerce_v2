
import 'dart:core';

class AddressList{
  AddressList();

  List<AddressDetail> addresses = [];

  Map<dynamic, dynamic> requestBody(){
    Map<dynamic, dynamic> body = <dynamic, dynamic>{};
    
    ///body['addresses'] = addresses;
    List<dynamic> addressDetails = [];
    addresses.forEach((address){
      Map addressDetail = address.requestBody();
      addressDetails.add(addressDetail);
    });

    body['addresses'] = addressDetails;

    return body;
  }  
}

class AddressDetail
{
  AddressDetail();

  String country='';
  String state = '';
  String city = '';
  String pincode = '';
  String address = '';
  int isDeliveryAddress = 0;
  String flagCode = 'in';

  Map<dynamic, dynamic> requestBody(){
    Map<dynamic, dynamic> body = <String, dynamic>{};
    
    body['country'] = country;
    body['state'] = state;
    body['city'] = city;
    body['pincode'] = pincode;
    body['address'] = address;
    body['isDeliveryAddress'] = isDeliveryAddress;
    body['flagCode'] = flagCode;

    return body;
  }
}

class UserDetail extends Object{
  UserDetail();

  String firstName = '';
  String lastName = '';
  String phone = '';
  String email = '';
  String password = '';
  String country ='';
  String state = '';
  String city = '';
  int pincode = 0;
  String address1 = '';
  String address2 = '';
  String store = '';
  String userRole = 'Supplier';
  bool subscribe = false;
  List<String> deliveryPinCodes = [];
  int moreAddressCount =0;
  String flagCode = 'in';
  AddressList address = AddressList();

  Map<dynamic, dynamic> requestBody()
  {
    Map<dynamic, dynamic> body = <String, dynamic>{};

    body['firstName'] = firstName;
    body['lastName'] = lastName;
    body['phone'] = phone;
    body['email'] = email;
    body['password'] = password;
    body['country'] = country;
    body['state'] = state;
    body['city'] = city;
    body['pincode'] = pincode;
    body['address1'] = address1;
    body['address2'] = address2;
    body['store'] = store;
    body['userRole'] = userRole;
    body['subscribe'] = subscribe;
    body['deliveryPinCodes'] = deliveryPinCodes;
    body['moreAddressCount'] = moreAddressCount;
    body['flagCode'] = flagCode;
    body['address'] = address.requestBody();

    return body;
  }
}

class TestDetail {
  TestDetail();

  String country ='';
  String state = '';
  String city = '';
  String pincode = '';
  String address = '';
  int isDeliveryAddress = 0;
  String flagCode = '';

  Map<dynamic, dynamic> requestBody()
  {
    Map<dynamic, dynamic> body = <String, dynamic>{};

    body['country'] = country;
    body['state'] = state;
    body['city'] = city;
    body['pincode'] = pincode;
    body['address'] = address;
    body['isDeliveryAddress'] = isDeliveryAddress;
    body['flagCode'] = flagCode;      

    return body;
  }  
}