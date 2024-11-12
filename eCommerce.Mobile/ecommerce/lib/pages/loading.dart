import 'package:flutter/material.dart';
import 'package:flutter_spinkit/flutter_spinkit.dart';
import 'package:localstorage/localstorage.dart';

import 'package:ecommerce/utils/util.dart';

class Loading extends StatefulWidget {
  const Loading({super.key});

  @override
  State<Loading> createState() => _LoadingState();
}

class _LoadingState extends State<Loading> {

  @override
  void initState() {
    super.initState();

    login();
  }
  
  @override
  void dispose() {
    super.dispose();
  }

  void login() async
  {
    await Future.delayed(const Duration(seconds: 4), (){
    });

    String? mobileNo = localStorage.getItem('mobileno');

    if (mobileNo != null && mobileNo!.isNotEmpty)
    {
      dynamic response = Util.getUserDetail();
      Navigator.pushReplacementNamed(context, '/manage', arguments: response);
    }
    else{
      Navigator.pushReplacementNamed(context, '/login');
    }
  }

  @override
  Widget build(BuildContext context) {

    return const Scaffold(
      backgroundColor: Color.fromRGBO(51, 155, 124, 1.0),
        body: Padding(
          padding: EdgeInsets.fromLTRB(15.0, 35.0, 15.0, 0.0),
          child: Column(
            crossAxisAlignment: CrossAxisAlignment.center,
            children: <Widget>[

              SizedBox(height: 36.0),

              Center( 
                child: SpinKitRipple(
                  color: Color.fromRGBO(245, 245, 220, 1.0),
                  size: 90.0,
                  borderWidth: 10.0,
                ),
              ),

              SizedBox(height: 36.0),

              Center(
                child: CircleAvatar(
                  backgroundImage: AssetImage(
                    'assets/images/bagwani.jpg'
                  ),
                  radius: 53.0,
                ),
              ),

              SizedBox(height: 13.0),

              Text(
                'GoGarden',
                style: TextStyle(
                  color: Color.fromRGBO(245, 245, 220, 1.0),
                  fontSize: 25,
                  fontWeight: FontWeight.bold,
                  letterSpacing: 1,
                ),
              ),              

              SizedBox(height: 122.0),

              Text(
                'from',
                style: TextStyle(
                  color: Color.fromRGBO(245, 245, 220, 1.0),
                  fontSize: 21,
                ),
              ),

              SizedBox(height: 21.0),

              Center(
                child: Image(
                  image: AssetImage(
                    'assets/images/mi-25.jpg'
                  ),
                ),
              ),

            ],
          ),
        ),
    );
  }
}