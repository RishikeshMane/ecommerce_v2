import 'package:flutter/material.dart';

import 'package:ecommerce/utils/style.dart';
import 'package:ecommerce/utils/util.dart';

class ManagePage extends StatelessWidget {

  final String title = 'Manage';

  const ManagePage({super.key, title});

  void onMyInventory(BuildContext context, dynamic args){
    Navigator.pushNamed(context, '/inventory', arguments: args);
  }

  void onOtherUsersInventory(BuildContext context, dynamic args){
    Navigator.pushNamed(context, '/existingproduct', arguments: args);
  }

  void onMyOrder(BuildContext context, dynamic args){

  }

  void onMyPayments(BuildContext context, dynamic args){

  }        
  
  // This widget is the root of your application.
  @override
  Widget build(BuildContext context) {

    dynamic args = ModalRoute.of(context)!.settings.arguments;

    return Scaffold(
      resizeToAvoidBottomInset: true,
      backgroundColor: Style.pageBackground,
      appBar: AppBar(
        title: Text(
          ///title,
          'Hello ${args['user']}',
          style: const TextStyle(
            color: Color.fromRGBO(245, 245, 220, 1.0),
            fontSize: 21,
          ),
          ),
        actions: <Widget>[  
          IconButton(
            icon: const Icon(
              Icons.logout,
              color: Colors.white,
              size: 35,
            ),
            onPressed: () {
              Util.deleteUserDetail();
              Navigator.pushReplacementNamed(context, '/login');
            },
          )
        ],          
        centerTitle: true,
        elevation: 0.0,
        backgroundColor: const Color.fromRGBO(51, 155, 124, 1.0),
      ),
      body:
        SafeArea(
          child: Container(
            decoration: const BoxDecoration(
              image: DecorationImage(
                image: AssetImage('assets/images/gogardenbg.jpg'),
                fit: BoxFit.cover,
              ),
            ),            
            child: Column(
              crossAxisAlignment: CrossAxisAlignment.start,
              children: <Widget>[
                const SizedBox(
                  height: 12,
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
                  height: 51,
                ),            
                Row(
                children: <Widget>[
                  Expanded(
                    flex: 10,
                    child: Container(
                      padding: const EdgeInsets.fromLTRB(4, 4, 4, 4),
                      color: Style.pageBackground,
                      child: ElevatedButton.icon(
                          style: ButtonStyle(
                            shape: WidgetStateProperty.all<RoundedRectangleBorder>(
                                      RoundedRectangleBorder(
                                        borderRadius: BorderRadius.circular(1.0),
                                        side: const BorderSide(color: Color.fromRGBO(245, 245, 220, 1.0))
                                      ),
                            ),
                            fixedSize: WidgetStateProperty.all<Size>(
                              const Size(double.infinity, 175),
                            ),                        
                            backgroundColor: WidgetStateProperty.all<Color>(
                              const Color.fromRGBO(51, 101, 205, 1.0)
                            ),
                            foregroundColor: WidgetStateProperty.all<Color>(
                              const Color.fromRGBO(245, 245, 220, 1.0),
                            ),
                          ),
                          icon: const Icon(
                                  Icons.bookmark_add,
                                  color: Color.fromRGBO(245, 245, 220, 1.0),
                                  size: 42,
                                ),
                          label: const Text(
                                          'My Inventory',
                                          style: TextStyle(
                                          color: Color.fromRGBO(245, 245, 220, 1.0),
                                          fontSize: 21,
                                        ),
                          ),
                          onPressed: () {
                            onMyInventory(context, args);
                          },
                        ),
                    ),
                  ),
                  Expanded(
                    flex: 10,
                    child: Container(
                      padding: const EdgeInsets.fromLTRB(4, 4, 4, 4),
                      color: Style.pageBackground,
                      child: ElevatedButton.icon(
                          style: ButtonStyle(
                            shape: WidgetStateProperty.all<RoundedRectangleBorder>(
                                      RoundedRectangleBorder(
                                        borderRadius: BorderRadius.circular(1.0),
                                        side: const BorderSide(color: Color.fromRGBO(245, 245, 220, 1.0))
                                      ),
                            ),
                            fixedSize: WidgetStateProperty.all<Size>(
                              const Size(double.infinity, 175),
                            ),
                            backgroundColor: WidgetStateProperty.all<Color>(
                              const Color.fromARGB(205, 227, 147, 10)
                            ),
                            foregroundColor: WidgetStateProperty.all<Color>(
                              const Color.fromRGBO(245, 245, 220, 1.0),
                            ),
                          ),
                          onPressed: () {
                            onOtherUsersInventory(context, args);
                          },
                          icon: const Icon(
                                  Icons.book,
                                  color: Color.fromRGBO(245, 245, 220, 1.0),
                                  size: 42,
                                ),                      
                          label: const Text(
                              'Other Supplier Inventory',
                              style: TextStyle(
                                      color: Color.fromRGBO(245, 245, 220, 1.0),
                                      fontSize: 21,
                                  ),                          
                            ),
                        ),
                    ),
                  ),
                ],
              ),
              const SizedBox(
                height: 2.0,
              ),            
              Row(
                children: <Widget>[
                  Expanded(
                    flex: 10,
                    child: Container(
                      padding: const EdgeInsets.fromLTRB(4, 4, 4, 4),
                      color: Style.pageBackground,
                      child: ElevatedButton.icon(
                          style: ButtonStyle(
                            shape: WidgetStateProperty.all<RoundedRectangleBorder>(
                                      RoundedRectangleBorder(
                                        borderRadius: BorderRadius.circular(1.0),
                                        side: const BorderSide(color: Color.fromRGBO(245, 245, 220, 1.0))
                                      ),
                            ),
                            fixedSize: WidgetStateProperty.all<Size>(
                              const Size(double.infinity, 175),
                            ),                        
                            backgroundColor: WidgetStateProperty.all<Color>(
                              const Color.fromARGB(255, 219, 71, 180)
                            ),
                            foregroundColor: WidgetStateProperty.all<Color>(
                              const Color.fromRGBO(245, 245, 220, 1.0),
                            ),
                          ),
                          onPressed: () {
                            onMyOrder(context, args);
                          },
                          icon: const Icon(
                                  Icons.border_outer_rounded,
                                  color: Color.fromRGBO(245, 245, 220, 1.0),
                                  size: 42,
                                ),                      
                          label: const Text(
                            'My Orders',
                            style: TextStyle(
                                        color: Color.fromRGBO(245, 245, 220, 1.0),
                                        fontSize: 21,
                                  ),
                          ),
                        ),
                    ),
                  ),
                  Expanded(
                    flex: 10,
                    child: Container(
                      padding: const EdgeInsets.fromLTRB(4, 4, 4, 4),
                      color: Style.pageBackground,
                      child: ElevatedButton.icon(
                          style: ButtonStyle(
                            shape: WidgetStateProperty.all<RoundedRectangleBorder>(
                                      RoundedRectangleBorder(
                                        borderRadius: BorderRadius.circular(1.0),
                                        side: const BorderSide(color: Color.fromRGBO(245, 245, 220, 1.0))
                                      ),
                            ),
                            fixedSize: WidgetStateProperty.all<Size>(
                              const Size(double.infinity, 175),
                            ),
                            backgroundColor: WidgetStateProperty.all<Color>(
                              const Color.fromARGB(253, 67, 66, 1),
                            ),
                            foregroundColor: WidgetStateProperty.all<Color>(
                              const Color.fromRGBO(245, 245, 220, 1.0),
                            ),
                          ),
                          onPressed: () {
                            onMyPayments(context, args);
                          },
                          icon: const Icon(
                                  Icons.payments,
                                  color: Color.fromRGBO(245, 245, 220, 1.0),
                                  size: 42,
                                ),                      
                          label: const Text(
                              'My Payments',
                              style: TextStyle(
                                      color: Color.fromRGBO(245, 245, 220, 1.0),
                                      fontSize: 21,
                                  ),                          
                            ),
                        ),
                    ),
                  ),
                ],
              ),
            ],
          ),
          ),
      ),
    );
  }
}
