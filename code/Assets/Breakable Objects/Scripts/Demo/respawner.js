var target:Transform;
var replace:Transform;
private var posBuffer:Vector3;
private var rotBuffer:Quaternion;

function Start (){

posBuffer=target.position;
rotBuffer=target.rotation;
}

function Update () {
if (target == null)

{
    var pos:Vector3 = posBuffer;

    if(replace!=null)
  target= gameObject.Instantiate(replace, pos, rotBuffer);
}
}