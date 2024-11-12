import 'package:flutter/material.dart';
import 'package:ecommerce/pages/subpages/inventoryproductdetailvariableimage.dart';

import 'package:ecommerce/models/productvariable.dart';
import 'package:ecommerce/utils/style.dart';
import 'package:ecommerce/utils/util.dart';

class InventoryProductDetailVariablePage extends StatefulWidget {

  final String userProductId;
  final ProductVariable productVariable;
  late void Function() updateShowImages;

  InventoryProductDetailVariablePage({super.key, required this.userProductId, required this.productVariable, required this.updateShowImages});

  @override
  State<InventoryProductDetailVariablePage> createState() => InventoryProductDetailVariableState();
}

class InventoryProductDetailVariableState extends State<InventoryProductDetailVariablePage> {
  
  // This widget is the root of your application.
  @override
  Widget build(BuildContext context) {

    List<List<String>> imageUrls = Util.convertImageUrls(widget.productVariable.imageUrls);

    return Card (
      margin: const EdgeInsets.fromLTRB(2.0, 6.0, 2.0, 6.0),
      color: const Color.fromRGBO(205, 205, 205, 1.0),
      child: Padding(
        padding: const EdgeInsets.fromLTRB(2.0, 2.0, 2.0, 2.0),
        child: Column(
          crossAxisAlignment: CrossAxisAlignment.stretch,
          children: <Widget>[
              const SizedBox(height: 4,),
              Row(
                mainAxisAlignment: MainAxisAlignment.center,

                children: <Widget>[
                  Expanded(
                    flex: 15,
                    child: Row(
                      mainAxisAlignment: MainAxisAlignment.start,
                      children: <Widget>[
                        const Text(
                          '  ',
                        ),

                        ColoredBox(
                          color: Color.fromRGBO(
                                          widget.productVariable.colorDetail.red,
                                          widget.productVariable.colorDetail.green,
                                          widget.productVariable.colorDetail.blue,
                                          1.0
                                        ),
                          child: Text(
                                  '   o',
                                  style: TextStyle(
                                          fontWeight: FontWeight.w800,
                                          fontSize: 16,
                                          color: Color.fromRGBO(
                                            widget.productVariable.colorDetail.red,
                                            widget.productVariable.colorDetail.green,
                                            widget.productVariable.colorDetail.blue,
                                            1.0
                                          ),
                                        ),
                                  ),
                        ),

                        Text(
                          '   ${widget.productVariable.colorDetail.description} ',
                          style: const TextStyle(
                                  fontWeight: FontWeight.w500,
                                  fontSize: 16,
                                  color: Color.fromRGBO(21, 21, 21, 1.0),
                          ), 
                        ),                        

                        Text(
                          ' ${widget.productVariable.sizeDetail.sizeCode} ',
                          style: const TextStyle(
                                  fontWeight: FontWeight.w700,
                                  fontSize: 16,
                                  color: Color.fromRGBO(21, 21, 21, 1.0),
                          ), 
                        ),
                      
                      ]
                    ),
                  ),

                  Expanded(
                    flex: 15,
                    child: Row(
                      mainAxisAlignment: MainAxisAlignment.center,
                      children: <Widget>[
                        ElevatedButton(
                          style: Style.enableFlatButton,
                          onPressed: (){
                            setState((){
                              widget.productVariable.imageShow = !widget.productVariable.imageShow;
                              ShowImage().setShowImage(widget.productVariable.index, widget.productVariable.imageShow);
                              widget.updateShowImages();
                            });
                          },
                          child: Text(
                                    (widget.productVariable.imageShow) ? 'Hide Images' : 'Show Images',
                                    style: const TextStyle(
                                      fontWeight: FontWeight.w800,
                                      fontSize: 16,
                          ),
                          ),
                        ),                        
                      ]
                    ),
                  ),
                ]
              ),

              Visibility(
                visible: widget.productVariable.imageShow,
                child: SingleChildScrollView(
                  child: Column(
                    crossAxisAlignment: CrossAxisAlignment.start,
                    children: imageUrls.map((imageUrls) =>
                      InventoryProductDetailVariableImage(userProductId: widget.userProductId, index: widget.productVariable.index, imageUrls: imageUrls),
                    ).toList(),        
                  ),
                ),
              ),
          ],
        ),
      ),
    );
  }
}
