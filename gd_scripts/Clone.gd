extends CharacterBody2D

@export var move_speed: float = 150.0
var _velocity: Vector2 = Vector2.ZERO

func _ready() -> void:
	add_to_group("PlayerGroup")

	print("Clone pronto: %s | Grupo: %s" % [name, is_in_group("PlayerGroup")])

	var players: Array = get_tree().get_nodes_in_group("PlayerGroup")
	for player in players:
		if player != self and player.has_node("Anim"):
			var player_sprite := player.get_node("Anim") as AnimatedSprite2D
			var clone_sprite := get_node("Anim") as AnimatedSprite2D
			clone_sprite.flip_h = player_sprite.flip_h
			break

	force_update_transform()
	get_node("CollisionShape2D").disabled = false

func _physics_process(delta: float) -> void:
	_velocity = velocity

	var direction: float = sign(_velocity.x)

	if abs(direction) > 0.1:
		var sprite := get_node("Anim") as AnimatedSprite2D
		sprite.flip_h = direction > 0
		print("Clone flip: %s" % sprite.flip_h)

	velocity = _velocity
	move_and_slide()
