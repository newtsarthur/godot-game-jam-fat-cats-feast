extends Area2D

@export var fall_speed: float = 300.0
@export var reload_tilemap: NodePath
@export var button2: NodePath
@export var cutscene_node: NodePath
@export var fade_node: NodePath
@export var next_scene: PackedScene  # ← Aqui está a cena que será exportada!

var _should_fall := false
var _original_position: Vector2
var has_touched_ground := false

func _ready():
	_original_position = position
	$CollisionShape2D.set_deferred("disabled", true)
	if not has_touched_ground:
		body_entered.connect(_on_body_entered)


func _physics_process(delta: float) -> void:
	if _should_fall:
		position += Vector2(0, fall_speed * delta)


func start_falling():
	_should_fall = true
	$CollisionShape2D.set_deferred("disabled", false)
	has_touched_ground = false


func reset_thorn():
	_should_fall = false
	position = _original_position
	$CollisionShape2D.set_deferred("disabled", true)


func _on_body_entered(body: Node2D) -> void:
	if body is TileMap or body.is_in_group("Ground") or body.is_in_group("Platform"):
		_should_fall = false
		has_touched_ground = true
		print("Thorn atingiu o chão/tilemap")

		var btn = get_node_or_null(button2)
		if btn and "mouse_death" in btn:
			if btn.mouse_death != 0:
				var cutscene = get_node_or_null(cutscene_node)
				var fade = get_node_or_null(fade_node)

				if fade:
					await fade.start_fade()

				if cutscene and cutscene.has_method("play_cutscene"):
					cutscene.play_cutscene()

					if not cutscene.is_connected("cutscene_finished", Callable(self, "_on_cutscene_finished")):
						cutscene.connect("cutscene_finished", Callable(self, "_on_cutscene_finished"))

					PlayerDataTw.items_collected_total += PlayerDataTw.items_collected_in_level
					PlayerDataTw.items_collected_in_level = 0

					print("Acabou o jogo")
				else:
					push_error("Falha ao encontrar ou castar para Cutscene.")
					print("Tipo real do nó: ", typeof(cutscene))
			else:
				print("Não acabou o jogo")
				var reload = get_node_or_null(reload_tilemap)
				if reload:
					reload.visible = true


func _on_cutscene_finished():
	print("Cutscene terminou. Mudando de cena...")
	var fade = get_node_or_null(fade_node)
	if fade:
		await fade.start_fade()

	if next_scene:
		get_tree().change_scene_to_packed(next_scene)
	else:
		push_error("Cena de próxima fase não definida!")
