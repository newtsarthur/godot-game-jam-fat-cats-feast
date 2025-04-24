extends Area2D

var blink_tween: Tween
var sprite: Sprite2D

func _ready() -> void:
	sprite = $Sprite2D  # Ajuste o caminho se necessário
	start_blink_idle()
	connect("body_entered", Callable(self, "_on_body_entered"))

func start_blink_idle() -> void:
	blink_tween = get_tree().create_tween()
	blink_tween.set_loops()  # Loop infinito

	# Pisca de 0 → 1
	blink_tween.tween_method(
		Callable(self, "set_shader_blink_intensity"),
		0.0,
		1.0,
		0.3
	).set_trans(Tween.TRANS_SINE).set_ease(Tween.EASE_IN_OUT)

	# E volta de 1 → 0
	blink_tween.tween_method(
		Callable(self, "set_shader_blink_intensity"),
		1.0,
		0.0,
		0.3
	).set_trans(Tween.TRANS_SINE).set_ease(Tween.EASE_IN_OUT)

func set_shader_blink_intensity(value: float) -> void:
	if sprite.material is ShaderMaterial:
		var shader_material := sprite.material as ShaderMaterial
		shader_material.set_shader_parameter("blink_intensity", value)

func _on_body_entered(body: Node) -> void:
	if body.is_in_group("PlayerGroup"):
		add_point_player()
		queue_free()

func add_point_player() -> void:
	PlayerDataTw.items_collected_in_level += 1
