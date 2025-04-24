extends Node2D

signal cutscene_finished

@export var animation_name: String = ""
@export var autoplay: bool = false

var _animation_player: AnimationPlayer
var _is_skipping := false

func _ready() -> void:
	_animation_player = $AnimationPlayer
	
	if autoplay:
		play_cutscene()

func play_cutscene() -> void:
	if _animation_player and _animation_player.has_animation(animation_name):
		_animation_player.animation_finished.connect(_on_animation_finished)
		_animation_player.play(animation_name)
	else:
		push_error("AnimationPlayer não encontrado ou animação inválida.")

func _on_animation_finished(name: StringName) -> void:
	if name == animation_name:
		print("Animação terminou, emitindo sinal...")
		emit_signal("cutscene_finished")

func _process(delta: float) -> void:
	if Input.is_action_just_pressed("cancel"):
		if not _is_skipping:
			_is_skipping = true
			print("Animação pulada!")
			emit_signal("cutscene_finished")
