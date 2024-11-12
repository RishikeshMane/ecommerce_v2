import 'package:flutter/material.dart';
import 'package:url_launcher/url_launcher.dart';

import 'package:ecommerce/utils/style.dart';
import 'package:ecommerce/validation/loginvalidation.dart';
import 'package:ecommerce/services/login.service.dart';
import 'package:ecommerce/utils/util.dart';

class LoginPage extends StatefulWidget {
  const LoginPage({super.key, required this.title});

  final String title;

  @override
  State<LoginPage> createState() => _LoginPageState();
}

class _LoginPageState extends State<LoginPage> {

  bool _hidePassword = true;

  Future<void> _showErrorDialog() async {
    return showDialog<void>(
      context: context,
      barrierDismissible: false, // user must tap button!
      builder: (BuildContext context) {
        return AlertDialog(
          title: const Text('Login Error !'),
          backgroundColor: const Color.fromRGBO(254, 254, 254, 1.0),
          content: const SingleChildScrollView(
            child: ListBody(
              children: <Widget>[
                Text('Cannot login. Try again.',
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

  Future<void> _showNonSupplierDialog() async {
    return showDialog<void>(
      context: context,
      barrierDismissible: false, // user must tap button!
      builder: (BuildContext context) {
        return AlertDialog(
          title: const Text('Non Supplier Error !'),
          backgroundColor: const Color.fromRGBO(254, 254, 254, 1.0),
          content: const SingleChildScrollView(
            child: ListBody(
              children: <Widget>[
                Text('Sorry ! You are not a Supplier.',
                    style: TextStyle(
                    fontSize: 20,
                  ),
                ),
                SizedBox(
                  height: 11.0,
                ),
                Text('If you are a Buyer go to our web store to place orders.',
                    style: TextStyle(
                    fontSize: 20,
                  ),
                ),                
              ],
            ),
          ),
          actions: <Widget>[

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

                  Center(
                    child: ElevatedButton(
                    style: Style.enableButton,
                    onPressed: (){
                      Navigator.of(context).pop();
                    },
                    child: const Text(
                              '  Ok  ',
                              style: TextStyle(
                                fontSize: 21,
                              ),
                            ),
                ),               
            ),
          ],
        );
      },
    );
  }

  void login() async{
    LoginService ls = LoginService();

    dynamic response = await ls.getLogin(Loginvalidation.login,
                                        Util.encrypt(Loginvalidation.password)
                                        );

    if (response['id'] == 'true' && response['role'] != 'Supplier')
    {
      await _showNonSupplierDialog();
    }
    else if (response['id'] == 'true')
    {
      Util.updateUserDetail(response, Util.encrypt(Loginvalidation.password));
      Navigator.pushReplacementNamed(context, '/manage', arguments: response);
    }
    else if (response['id'] == 'false')
    {
      await _showErrorDialog();
    }
  }

  register(){
    Navigator.pushNamed(context, '/register');
  }  

  @override
  Widget build(BuildContext context) {

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
              height: 12.0,
            ),
                        
            const Center(
              child: CircleAvatar(
                backgroundColor: Color.fromRGBO(51, 155, 124, 1.0),
                radius: 27.0,
                child: CircleAvatar(
                  backgroundImage: AssetImage('assets/images/bagwanism.jpg'),
                  radius: 25.0,
                ),
              ),
            ),
            const SizedBox(
              height: 21.0,
            ),            
            const Text(
              'MobileNo',
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
                prefixIcon: const Icon(Icons.mobile_screen_share),
                filled: true,
                fillColor: Style.textBackground,
                border: OutlineInputBorder(
                  borderRadius: BorderRadius.circular(10.0),
                ),
                hintText: '9898989898',
              ),
              
              controller: Loginvalidation.loginController,
              ///focusNode: Loginvalidation.loginFocusNode,
              onChanged: (String value){
                            setState(() {
                              Loginvalidation.login = Loginvalidation.loginController.text;                 
                          });},           
            ),

            const SizedBox(
              height: 21.0,
            ),
            const Text(
              'Password',
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
              obscuringCharacter: '*',
              obscureText: _hidePassword,
              decoration: InputDecoration(
                prefixIcon: const Icon(Icons.keyboard),
                filled: true,
                fillColor: Style.textBackground,                
                border: OutlineInputBorder(
                  borderRadius: BorderRadius.circular(10.0),
                ),
                hintText: '********',
              ),
                            
              controller: Loginvalidation.passwordController,
              ///focusNode: Loginvalidation.passwordFocusNode,
              onChanged: (String value){
                            setState(() {
                              Loginvalidation.password = Loginvalidation.passwordController.text;                 
                          });},              
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

            const SizedBox(
              height: 21.0,
            ),            

            Center(
              child: ElevatedButton(
                      style: ButtonStyle(
                        shape: WidgetStateProperty.all<RoundedRectangleBorder>(
                                  RoundedRectangleBorder(
                                    borderRadius: BorderRadius.circular(22.0),
                                    side: const BorderSide(color: Color.fromRGBO(51, 155, 124, 1.0))
                                  ),
                        ),
                        backgroundColor: WidgetStateProperty.all<Color>(
                          const Color.fromRGBO(51, 155, 124, 1.0)
                        ),
                        foregroundColor: WidgetStateProperty.all<Color>(
                          const Color.fromRGBO(245, 245, 220, 1.0),
                        ),
                      ),
                      onPressed: () {
                        login();
                      },
                      child: const Text('  Login  '),
                    ),               
            ),

            const SizedBox(
              height: 5.0,
            ),            

            Center(
              child: ElevatedButton(             
                      style: ButtonStyle(
                        shape: WidgetStateProperty.all<RoundedRectangleBorder>(
                                  RoundedRectangleBorder(
                                    borderRadius: BorderRadius.circular(22.0),
                                    side: const BorderSide(color: Color.fromRGBO(51, 155, 124, 1.0))
                                  ),
                        ),                        
                        backgroundColor: WidgetStateProperty.all<Color>(
                          const Color.fromRGBO(51, 155, 124, 1.0)
                        ),
                        foregroundColor: WidgetStateProperty.all<Color>(
                          const Color.fromRGBO(245, 245, 220, 1.0),
                        ),
                      ),
                      onPressed: () {
                        register();
                      },
                      child: const Text('  Register  '),
                    ),               
            ),

          ],
        ),
      ),
    );
  }
}
