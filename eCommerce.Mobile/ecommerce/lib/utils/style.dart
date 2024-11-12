
import 'package:flutter/material.dart';

class Style {

  static ButtonStyle enableButton = ButtonStyle(
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
        );

  static ButtonStyle enableFlatButton = ButtonStyle(
          shape: WidgetStateProperty.all<RoundedRectangleBorder>(
                    RoundedRectangleBorder(
                      borderRadius: BorderRadius.circular(15.0),
                      side: const BorderSide(color: Color.fromRGBO(51, 155, 124, 1.0))
                    ),
          ),    
          backgroundColor: WidgetStateProperty.all<Color>(
            const Color.fromRGBO(51, 155, 124, 1.0)
          ),
          foregroundColor: WidgetStateProperty.all<Color>(
            const Color.fromRGBO(245, 245, 220, 1.0),
          ),    
        );

  static ButtonStyle borderFlatButton = ButtonStyle(
          shape: WidgetStateProperty.all<RoundedRectangleBorder>(
                    RoundedRectangleBorder(
                      borderRadius: BorderRadius.circular(15.0),
                      side: const BorderSide(color: Color.fromRGBO(21, 21, 21, 1.0))
                    ),
          ),    
          backgroundColor: WidgetStateProperty.all<Color>(
            const Color.fromRGBO(51, 155, 124, 1.0)
          ),
          foregroundColor: WidgetStateProperty.all<Color>(
            const Color.fromRGBO(245, 245, 220, 1.0),
          ),    
        );                

  static ButtonStyle disableButton = ButtonStyle(
          shape: WidgetStateProperty.all<RoundedRectangleBorder>(
                    RoundedRectangleBorder(
                      borderRadius: BorderRadius.circular(22.0),
                      side: const BorderSide(color: Color.fromRGBO(125, 125, 125, 1.0))
                    ),
          ),    
          backgroundColor: WidgetStateProperty.all<Color>(
            const Color.fromRGBO(125, 125, 125, 1.0)
          ),
          foregroundColor: WidgetStateProperty.all<Color>(
            const Color.fromRGBO(245, 245, 220, 1.0),
          ),    
        );

  static Color pageBackground = const Color.fromRGBO(228, 228, 228, 1.0);

  static Color textBackground = const Color.fromRGBO(254, 254, 254, 1.0);             
}