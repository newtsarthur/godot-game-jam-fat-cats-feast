extends CharacterBody2D

signal pontuacao_atualizada(nova_pontuacao)
signal clones_atualizados(clones_disponiveis)

@export var clone: PackedScene
@export var layout_scene: PackedScene
@export var max_clones: int
@export var move_speed: float = 150.0
@export var jump_force: float = 350.0
@export var gravity: float = 1000.0
@export var max_fall_speed: float = 1000.0
@export var jump_cut_multiplier: float = 0.5

var _velocity: Vector2
var _is_dead: bool = false
var _pontuacao: int = 0
var layout_instance: Node = null

# Variáveis de controle de clones
@export var clones_current: int = 0
@export var clones_max: int = 0
var _active_clones: Array = []

func _ready():
	if layout_scene:
		var layout_node = layout_scene.instantiate()
		add_child(layout_node)
		layout_instance = layout_node.find_child("Control", true, false)
		if layout_instance:
			print(max_clones)
			print(clones_current)
			print("Funciona?")
			layout_instance.set_player(self)
		else:
			push_error("HUD (Control com script) não encontrado na cena Layout!")

func get_clones_current() -> int:
	return clones_current

func get_clones_max() -> int:
	return clones_max
	
func adicionar_pontos(pontos: int):
	_pontuacao += (max_clones - clones_current)
	emit_signal("pontuacao_atualizada", _pontuacao)

func _physics_process(delta):
	var direction = Input.get_action_strength("move_right") - Input.get_action_strength("move_left")
	_velocity.x = direction * move_speed

	if direction != 0:
		$Anim.flip_h = direction > 0

	if not is_on_floor():
		_velocity.y += gravity * delta
		if _velocity.y > max_fall_speed:
			_velocity.y = max_fall_speed
	else:
		_velocity.y = 0
		if Input.is_action_just_pressed("jump"):
			_velocity.y = -jump_force
		if Input.is_action_just_pressed("summon_clone"):
			summon_clone()

	if Input.is_action_just_released("jump") and _velocity.y < 0:
		_velocity.y *= jump_cut_multiplier

	if Input.is_action_just_pressed("reload"):
		PlayerDataTw.instance.items_collected_in_level = 0
		get_node("../Fade").start_death_fade(_on_death_fade_complete)

	velocity = _velocity
	move_and_slide()

	for i in range(get_slide_collision_count()):
		if _is_dead:
			break
		var collision = get_slide_collision(i)
		if collision.get_collider().is_in_group("EnemyGroup"):
			_is_dead = true
			PlayerDataTw.instance.death_count += 1
			print("Você morreu %d vezes!" % PlayerDataTw.instance.death_count)
			print("Player morreu")
			get_node("../Fade").start_death_fade(_on_death_fade_complete)

	clones_max = max_clones

func _on_death_fade_complete():
	print("Fade completo. Recarregando cena...")
	reload_scene()

func reload_scene():
	var current_scene_path = get_tree().current_scene.scene_file_path
	get_tree().change_scene_to_file(current_scene_path)
	print("Cena recarregada")

func summon_clone():
	if clones_current >= max_clones:
		print("Limite de clones atingido")
		return
	if clone == null:
		push_error("Cena do clone não atribuída!")
		return
	var new_clone = clone.instantiate()
	if new_clone:
		clones_current += 1  # Atualizando clones_current diretamente
	if new_clone is CharacterBody2D:
		new_clone.name = "PlayerClone"
		new_clone.add_to_group("PlayerGroup")
		new_clone.global_position = global_position
		if new_clone.has_node("Anim"):
			new_clone.get_node("Anim").flip_h = $Anim.flip_h
		get_parent().add_child(new_clone)
		new_clone.force_update_transform()
		print("Clone CharacterBody2D criado: %s, Flip: %s" % [new_clone.name, str($Anim.flip_h)])
	elif new_clone is Node2D:
		new_clone.name = "PlayerClone"
		new_clone.add_to_group("PlayerGroup")
		new_clone.global_position = global_position
		if new_clone.has_node("Anim"):
			new_clone.get_node("Anim").flip_h = $Anim.flip_h
		get_parent().add_child(new_clone)
		new_clone.force_update_transform()
		print("Clone Node2D criado: %s, Flip: %s" % [new_clone.name, str($Anim.flip_h)])
	else:
		push_error("Tipo de clone não suportado: %s" % typeof(new_clone))
	emit_signal("clones_atualizados", clones_current)  # Emitir o número atualizado de clones
