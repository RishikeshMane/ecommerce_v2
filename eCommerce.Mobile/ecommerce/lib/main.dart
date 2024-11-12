import 'package:ecommerce/pages/inventory.dart';
import 'package:ecommerce/pages/inventoryproductdetail.dart';
import 'package:flutter/material.dart';
import 'package:localstorage/localstorage.dart';

import 'package:ecommerce/pages/loading.dart';
import 'package:ecommerce/pages/login.dart';
import 'package:ecommerce/pages/register.dart';
import 'package:ecommerce/pages/manage.dart';
import 'package:ecommerce/pages/inventory.dart';
import 'package:ecommerce/pages/existingproduct.dart';
import 'package:ecommerce/pages/existingproductdetail.dart';

void main() async {
  WidgetsFlutterBinding.ensureInitialized();
  await initLocalStorage();
  
  runApp(const ECommerceApp());
}

class ECommerceApp extends StatelessWidget {
  const ECommerceApp({super.key});

  // This widget is the root of your application.
  @override
  Widget build(BuildContext context) {
    return MaterialApp(
      title: 'eCommerce',
      theme: ThemeData(
        colorScheme: ColorScheme.fromSeed(seedColor: Colors.deepPurple),
        useMaterial3: true,
      ),
      
      initialRoute: '/',
      routes: {
        '/' : (context) => const Loading(),
        '/login' : (context) => const LoginPage(title: 'Bagwani'),
        '/register' : (context) => const RegisterPage(title: 'Register'),
        '/manage' : (context) => const ManagePage(title: 'Manage'),
        '/inventory' : (context) => const InventoryPage(title: 'My Inventory'),
        '/existingproduct' : (context) => const ExistingProductPage(title: 'Other Supplier Products'),
        '/existingproductdetail' : (context) => const ExistingProductDetailPage(title: 'Product Details'),
        '/inventoryproductdetail' : (context) => const InventoryProductDetailPage(title: 'Product Details'),
      },      
    );
  }
}
