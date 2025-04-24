extends CanvasLayer

@onready var fade: ColorRect = $ColorRect

func _ready() -> void:
	if not fade:
		push_error("Fade ColorRect not found!")
		return
	
	fade.size = get_viewport().get_visible_rect().size
	fade.modulate = Color(33.0 / 255.0, 30.0 / 255.0, 51.0 / 255.0, 0)
	fade.visible = false

func start_death_fade(callback: Callable = Callable()) -> void:
	if not is_instance_valid(fade):
		push_error("Fade node is not valid")
		return
	
	fade.visible = true

	var tween := create_tween()
	tween.tween_property(
		fade, "modulate",
		Color(33.0 / 255.0, 30.0 / 255.0, 51.0 / 255.0, 1),
		0.5
	)
	tween.tween_callback(_on_fade_complete.bind(callback))

func _on_fade_complete(callback: Callable) -> void:
	print("Tela escurecida. Recarregando cena...")

	# Executa callback se for válido
	if callback.is_valid():
		callback.call()

	var tree := get_tree()
	if tree:
		# Verifica se `reload_current_scene` está disponível
		if tree.has_method("reload_current_scene"):
			tree.call_deferred("reload_current_scene")
		else:
			# Fallback: tentar recarregar usando o caminho da cena atual
			var current_scene := tree.get_current_scene()
			if current_scene and current_scene.scene_file_path:
				tree.change_scene_to_file(current_scene.scene_file_path)
			else:
				push_error("Não foi possível recarregar a cena atual!")
	else:
		push_error("SceneTree não disponível!")

func start_fade() -> void:
	if not is_instance_valid(fade):
		push_error("Fade node is not valid")
		return
	
	fade.visible = true

	var tween := create_tween()
	tween.tween_property(
		fade, "modulate",
		Color(33.0 / 255.0, 30.0 / 255.0, 51.0 / 255.0, 1),
		0.5
	)

	await tween.finished

	print("Tela escurecida (await).")
	fade.modulate = Color(33.0 / 255.0, 30.0 / 255.0, 51.0 / 255.0, 0)
	fade.visible = false
