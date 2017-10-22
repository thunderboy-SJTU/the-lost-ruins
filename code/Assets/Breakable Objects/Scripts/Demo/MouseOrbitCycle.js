var gameObjectArray : GameObject[];
var distance = 3.0;
var xSpeed = 250.0;
var ySpeed = 120.0;
var yMinLimit = -360;
var yMaxLimit = 360;
var zoomRate = 25;

private var currentTarget;
private var x = 0.0;
private var y = 0.0;
private var currentCounter =0;

function Start () {
    var angles = transform.eulerAngles;
    x = angles.y;
    y = angles.x;
    currentTarget = gameObjectArray[0].transform;
}

function Update () {
	if (gameObjectArray) {
		if(Input.GetMouseButtonUp(0)){
		if(gameObjectArray.length > currentCounter+1)
		currentCounter++;
		else
		currentCounter =0;
		currentTarget = gameObjectArray[currentCounter].transform;
		
		}
		
		
		x += Input.GetAxis("Mouse X") * xSpeed * 0.02;
        y -= Input.GetAxis("Mouse Y") * ySpeed * 0.02;
        distance += -(Input.GetAxis("Mouse ScrollWheel") * Time.deltaTime) * zoomRate * Mathf.Abs(distance);      		
 		y = ClampAngle(y, yMinLimit, yMaxLimit);
        var rotation = Quaternion.Euler(y, x, 0);
        var position = rotation * Vector3(0.0, 0.0, -distance) + currentTarget.position;      
        transform.rotation = rotation;
        transform.position = position;  
    }
}

static function ClampAngle (angle : float, min : float, max : float) {
	if (angle < -360)
		angle += 360;
	if (angle > 360)
		angle -= 360;
	return Mathf.Clamp (angle, min, max);
}