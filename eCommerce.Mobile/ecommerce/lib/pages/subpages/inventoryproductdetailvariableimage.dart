import 'package:flutter/material.dart';

import 'package:ecommerce/utils/util.dart';
import 'package:ecommerce/services/image.service.dart';

class InventoryProductDetailVariableImage extends StatefulWidget {

  final List<String> imageUrls;
  final String userProductId;
  final int index;

  const InventoryProductDetailVariableImage({super.key, required this.userProductId, required this.index, required this.imageUrls});

  @override
  State<InventoryProductDetailVariableImage> createState() => InventoryProductDetailVariableImageState();
}

class InventoryProductDetailVariableImageState extends State<InventoryProductDetailVariableImage> {
  
  List<String> _imageUrlPaths = [
                                  '${Util.filescheme}://${Util.filehost}:${Util.fileport}/FileSystem/Product/Home~blank-flutter.jpg',
                                  '${Util.filescheme}://${Util.filehost}:${Util.fileport}/FileSystem/Product/Home~blank-flutter.jpg',
                                  '${Util.filescheme}://${Util.filehost}:${Util.fileport}/FileSystem/Product/Home~blank-flutter.jpg',
                                ];
  @override
  void initState() {
    updateImageUrlPath();
    super.initState();
  }

  Future<void> updateImageUrlPath() async{

    List<String> imageUrlPaths = [];

    for (int counter = 0; counter < widget.imageUrls.length; ++counter)
    {
      String imageUrl = 'FileSystem/Product/${widget.userProductId}~${widget.index}~${widget.imageUrls[counter]}';
      ImageService isv = ImageService();
      await isv.fileExits(imageUrl);
      if (isv.fileExist == true){
        imageUrl = '${Util.filescheme}://${Util.filehost}:${Util.fileport}/${imageUrl}';
        imageUrlPaths.add(imageUrl);
      }
      else{
        imageUrl = '${Util.filescheme}://${Util.filehost}:${Util.fileport}/FileSystem/Product/Home~blank-flutter.jpg';
        imageUrlPaths.add(imageUrl);
      }
    };

    if (imageUrlPaths.isEmpty){
      String imageUrl = '${Util.filescheme}://${Util.filehost}:${Util.fileport}/FileSystem/Product/Home~blank-flutter.jpg';
      imageUrlPaths.add(imageUrl);
    }    

    if (imageUrlPaths.length == 1){
      String imageUrl = '${Util.filescheme}://${Util.filehost}:${Util.fileport}/FileSystem/Product/Home~blank-flutter.jpg';
      imageUrlPaths.add(imageUrl);
    }

    if (imageUrlPaths.length == 2){
      String imageUrl = '${Util.filescheme}://${Util.filehost}:${Util.fileport}/FileSystem/Product/Home~blank-flutter.jpg';
      imageUrlPaths.add(imageUrl);
    }
    try{
      setState(() {
        _imageUrlPaths = [];
        _imageUrlPaths = imageUrlPaths;
      });
    }
    catch(err){}
  }

  
  // This widget is the root of your application.
  @override
  Widget build(BuildContext context) {

    updateImageUrlPath();

    return Card (
      margin: const EdgeInsets.fromLTRB(2.0, 6.0, 2.0, 6.0),
      color: const Color.fromRGBO(205, 205, 205, 1.0),
      child: 
        Padding(
          padding: const EdgeInsets.fromLTRB(2.0, 12.0, 2.0, 12.0),
          child: 
              Row(
                mainAxisAlignment: MainAxisAlignment.center,
                children: <Widget>[
                  Expanded(
                    flex: 10,
                    child:
                    Row(
                      mainAxisAlignment: MainAxisAlignment.center,
                      children: <Widget>[
                        Center(
                          child: Image(
                            fit: BoxFit.fill,
                            width: 109,
                            height: 127,
                            image: NetworkImage(
                              _imageUrlPaths[0],
                            ),
                          ),
                        ),                 
                      ]
                    ),
                  ),

                  Expanded(
                    flex: 10,
                    child: Row(
                      mainAxisAlignment: MainAxisAlignment.center,
                      children: <Widget>[
                        Center(
                          child: Image(
                            fit: BoxFit.fill,
                            width: 109,
                            height: 127,
                            image: NetworkImage(
                              _imageUrlPaths[1],
                            ),
                          ),
                        ),                  
                      ]
                    ),
                  ),

                  Expanded(
                    flex: 10,
                    child: Row(
                      mainAxisAlignment: MainAxisAlignment.center,
                      children: <Widget>[
                        Center(
                          child: Image(
                            fit: BoxFit.fill,
                            width: 109,
                            height: 127,
                            image: NetworkImage(
                              _imageUrlPaths[2],
                            ),
                          ),
                        ),                 
                      ]
                    ),
                  ),
                ]
              ),
          ),
    );
  }
}
