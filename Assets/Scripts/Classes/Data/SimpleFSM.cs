using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Reflection;
using System.Linq;

[Serializable]
public class SimpleFSM<T> where T : struct, IConvertible, IComparable
{
    [SerializeField]
    private T _state;
    private object _instance;
    [SerializeField]
    private bool _running;
    private Dictionary<T, StateFunctions> _stateFunctionLookup;

    private MethodBase Enter { get { return _stateFunctionLookup.ContainsKey(_state) ? _stateFunctionLookup[_state].Enter : null; } }
    private MethodBase Update { get { return _stateFunctionLookup.ContainsKey(_state) ? _stateFunctionLookup[_state].Update : null; } }
    private MethodBase Exit { get { return _stateFunctionLookup.ContainsKey(_state) ? _stateFunctionLookup[_state].Exit : null; } }

    public T State { get { return _state; } }
    public delegate void StateChange(T oldState, T newState);
    public event StateChange StateChanged;
    public bool IsRunning { get { return _running; } }

    private static Dictionary<Type, IEnumerable<MethodInfo>> _reflectionCache = new Dictionary<Type, IEnumerable<MethodInfo>>();
    private static Dictionary<Type, Array> _enumCache = new Dictionary<Type, Array>();

    private struct StateFunctions
    {
        public MethodBase Enter, Update, Exit;
    }

    public SimpleFSM(object instance)
    {
        if (!typeof(T).IsEnum) throw new Exception("T must be an Enum!");

        _instance = instance;

        _stateFunctionLookup = new Dictionary<T, StateFunctions>();

        if (!_reflectionCache.ContainsKey(typeof(T))) InitNew();
        else InitCached();
    }

    private void InitNew()
    {
        var methods = _instance.GetType().GetMethods(BindingFlags.NonPublic | BindingFlags.Instance).Where((x) => x.Name.StartsWith("_"));

        _reflectionCache.Add(typeof(T), methods);
        _enumCache.Add(typeof(T), Enum.GetValues(typeof(T)));

        foreach (var state in _enumCache[typeof(T)])
        {
            var stateMethods = methods.Where((x) => x.Name.Contains(state.ToString()));

            var stateFunctions = new StateFunctions();

            foreach (var method in stateMethods)
            {
                var name = method.Name;

                if (name.EndsWith("_Enter")) stateFunctions.Enter = method;
                else if (name.EndsWith("_Update")) stateFunctions.Update = method;
                else if (name.EndsWith("_Exit")) stateFunctions.Exit = method;
            }

            _stateFunctionLookup[(T)state] = stateFunctions;
        }
    }

    private void InitCached()
    {
        var methods = _reflectionCache[typeof(T)];

        foreach (var state in _enumCache[typeof(T)])
        {
            var stateMethods = methods.Where((x) => x.Name.Contains(state.ToString()));

            var stateFunctions = new StateFunctions();

            foreach (var method in stateMethods)
            {
                var name = method.Name;

                if (name.EndsWith("_Enter")) stateFunctions.Enter = method;
                else if (name.EndsWith("_Update")) stateFunctions.Update = method;
                else if (name.EndsWith("_Exit")) stateFunctions.Exit = method;
            }

            _stateFunctionLookup[(T)state] = stateFunctions;
        }
    }

    public void StartFSM(T initialState)
    {
        _running = true;
        _state = initialState;

        SetState(initialState, true);
    }

    public void UpdateFSM()
    {
        if (!_running) return;

        Update?.Invoke(_instance, null);
    }

    public void StopFSM()
    {
        _running = false;
    }

    public void SetState(T state, bool initialState = false)
    {
        if (!_running)
        {
            _state = state;
            return;
        }

        if (!initialState) Exit?.Invoke(_instance, null);

        _state = state;

        Enter?.Invoke(_instance, null);

        StateChanged?.Invoke(_state, state);
    }
}