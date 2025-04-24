extends CharacterBody2D

@export var grid: TileMapLayer
@export var move_speed: float = 25.0
@export var gravity: float = 0.0
@export var ray_length: float = 30.0

var _direction: int = 1
var _floor_detector: RayCast2D
var _wall_detector: RayCast2D
var _sprite: Sprite2D
var _collision_shape: CollisionShape2D

func _ready() -> void:
	_floor_detector = $FloorDetector
	_wall_detector = $WallDetector
	_sprite = $Sprite2D
	_collision_shape = $CollisionShape2D

	_setup_collision_layers()

	if _floor_detector == null:
		_floor_detector = RayCast2D.new()
		_floor_detector.name = "FloorDetector"
		add_child(_floor_detector)
		_floor_detector.target_position = Vector2(ray_length * _direction, 20)
		_floor_detector.enabled = true

	if _wall_detector == null:
		_wall_detector = RayCast2D.new()
		_wall_detector.name = "WallDetector"
		add_child(_wall_detector)
		_wall_detector.target_position = Vector2(ray_length * _direction, 0)
		_wall_detector.enabled = true

	for node in get_tree().get_nodes_in_group("PlayerGroup"):
		if node is CollisionObject2D:
			_floor_detector.add_exception(node)
			_wall_detector.add_exception(node)

	for node in get_tree().get_nodes_in_group("BabyCatGroup"):
		if node is CollisionObject2D:
			_floor_detector.add_exception(node)
			_wall_detector.add_exception(node)

func _setup_collision_layers() -> void:
	var player_layer: int = 1 << 0 # Layer 1
	var babycat_layer: int = 1 << 1 # Layer 2
	collision_mask &= ~player_layer
	collision_mask &= ~babycat_layer

func _physics_process(delta: float) -> void:
	var velocity = self.velocity

	if not is_on_floor():
		velocity.y += gravity * delta
	else:
		velocity.y = 0.0

	velocity.x = move_speed * _direction

	_update_raycasts()
	_check_for_turn()

	if _sprite:
		_sprite.flip_h = _direction > 0

	self.velocity = velocity
	move_and_slide()

func _update_raycasts() -> void:
	if _floor_detector:
		_floor_detector.target_position = Vector2(ray_length * _direction, 20)
		_floor_detector.force_raycast_update()

	if _wall_detector:
		_wall_detector.target_position = Vector2(ray_length * _direction, 0)
		_wall_detector.force_raycast_update()

func _check_for_turn() -> void:
	var should_turn := false

	if _floor_detector and not _floor_detector.is_colliding():
		should_turn = true

	if _wall_detector and _wall_detector.is_colliding():
		should_turn = true

	if should_turn:
		_direction *= -1
		_update_raycasts()
