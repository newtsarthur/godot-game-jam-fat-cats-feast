extends CharacterBody2D

@export var grid: TileMapLayer
@export var mouse_enemy: Array[CharacterBody2D]
@export var move_speed: float = 30.0
@export var gravity: float = 980.0
@export var ray_length: float = 30.0
@export var button88: NodePath
@export var player: NodePath

var _direction: int = 1
var _floor_detector: RayCast2D
var _wall_detector: RayCast2D
var _sprite: AnimatedSprite2D
var _hurtbox: Area2D


func _ready():
	_floor_detector = $FloorDetector
	_wall_detector = $WallDetector
	_sprite = $AnimatedSprite2D
	_hurtbox = $Hurtbox

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

	_hurtbox.area_entered.connect(_on_area_entered)


func _physics_process(delta):
	var velocity = self.velocity

	if not is_on_floor():
		velocity.y += gravity * delta
	else:
		velocity.y = 0

	velocity.x = move_speed * _direction

	check_for_turn()

	if _sprite:
		_sprite.flip_h = _direction > 0
		_sprite.play("walk")

	self.velocity = velocity


func _update_raycasts():
	if _floor_detector:
		_floor_detector.target_position = Vector2(ray_length * _direction, 20)
		_floor_detector.force_raycast_update()
		if _floor_detector.is_colliding():
			var collider = _floor_detector.get_collider()
			if collider and collider.is_in_group("PlayerGroup"):
				_floor_detector.add_exception(collider)

	if _wall_detector:
		_wall_detector.target_position = Vector2(ray_length * _direction, 0)
		_wall_detector.force_raycast_update()
		if _wall_detector.is_colliding():
			var collider = _wall_detector.get_collider()
			if collider and collider.is_in_group("PlayerGroup"):
				_wall_detector.add_exception(collider)


func check_for_turn():
	var should_turn = false

	if _floor_detector and not _floor_detector.is_colliding():
		should_turn = true

	if _wall_detector and _wall_detector.is_colliding():
		should_turn = true

	if should_turn:
		_direction *= -1
		_update_raycasts()


func _on_area_entered(area: Area2D):
	if area.is_in_group("ThornGroup") and "has_touched_ground" in area and not area.has_touched_ground:
		var btn = get_node_or_null(button88)
		if btn and "mouse_death" in btn:
			btn.mouse_death += 1
			print("Rato mortes: %s" % btn.mouse_death)
			print("Rato morreu (Area2D)")

		if grid:
			grid.enabled = false

		var player_node = get_node_or_null(player)
		if player_node and player_node is CharacterBody2D:
			for mouse in mouse_enemy:
				if mouse:
					mouse.queue_free()

			player_node.move_speed = 0
			player_node.jump_force = 0

		queue_free()
