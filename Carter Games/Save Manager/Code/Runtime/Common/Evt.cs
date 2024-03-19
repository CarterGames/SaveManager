/*
 * Copyright (c) 2024 Carter Games
 *
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 *
 * The above copyright notice and this permission notice shall be included in
 * all copies or substantial portions of the Software.
 *
 *
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NON-INFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 * THE SOFTWARE.
 */

using System;
using System.Collections.Generic;

namespace CarterGames.Common
{
    /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────────
    |   No Parameters Evt
    ───────────────────────────────────────────────────────────────────────────────────────────────────────────────── */
    
    
    /// <summary>
    /// A custom event class that helps avoid over subscription and more.
    /// </summary>
    public sealed class Evt
    {
        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Fields
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */
        
        private readonly Dictionary<string, Action> anonymous = new Dictionary<string, Action>();
        private event Action Action = delegate { };

        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Methods
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */
        
        /// <summary>
        /// Raises the event to all listeners.
        /// </summary>
        public void Raise()
        {
            Action?.Invoke();
        }

        
        /// <summary>
        /// Adds the action/method to the event listeners.
        /// </summary>
        /// <param name="listener">The listener to add.</param>
        public void Add(Action listener)
        {
            Action -= listener;
            Action += listener;
        }
        

        /// <summary>
        /// Adds the action/method to the event listeners.
        /// </summary>
        /// <param name="id">The id to refer to this listener.</param>
        /// <param name="listener">The listener to add.</param>
        public void AddAnonymous(string id, Action listener)
        {
            if (anonymous.TryGetValue(id, out var anon))
            {
                Add(anon);
                return;
            }
            
            anonymous.Add(id, listener);
            Add(anonymous[id]);
        }
        

        /// <summary>
        /// Removes the action/method from the event listeners.
        /// </summary>
        /// <param name="listener">The listener to remove.</param>
        public void Remove(Action listener)
        {
            Action -= listener;
        }

        
        /// <summary>
        /// Removes the action/method from the event listeners.
        /// </summary>
        /// <param name="id">The id of the listener to remove.</param>
        public void RemoveAnonymous(string id)
        {
            if (!anonymous.ContainsKey(id)) return;
            Remove(anonymous[id]);
            anonymous.Remove(id);
        }
        
        
        /// <summary>
        /// Clears all listeners from the event.
        /// </summary>
        public void Clear() 
        {
            anonymous.Clear();
            Action = null;
        }
    }
    
    
    /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────────
    |   1 Parameter Evt
    ───────────────────────────────────────────────────────────────────────────────────────────────────────────────── */
    
    
    /// <summary>
    /// A custom event class that helps avoid over subscription and more.
    /// </summary>
    public sealed class Evt<T>
    {
        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Fields
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */
        
        private readonly Dictionary<string, Action<T>> anonymous = new Dictionary<string, Action<T>>();
        private event Action<T> Action = delegate { };
        
        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Methods
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */
        
        /// <summary>
        /// Raises the event to all listeners.
        /// </summary>
        /// <param name="param">The params to pass through when raising.</param>
        public void Raise(T param)
        {
            Action?.Invoke(param);
        }

        
        /// <summary>
        /// Adds the action/method to the event listeners.
        /// </summary>
        /// <param name="listener">The listener to add.</param>
        public void Add(Action<T> listener)
        {
            Action -= listener;
            Action += listener;
        }
        
        
        /// <summary>
        /// Adds the action/method to the event listeners.
        /// </summary>
        /// <param name="id">The id to refer to this listener.</param>
        /// <param name="listener">The listener to add.</param>
        public void AddAnonymous(string id, Action<T> listener)
        {
            if (anonymous.TryGetValue(id, out var anon))
            {
                Add(anon);
                return;
            }
            
            anonymous.Add(id, listener);
            Add(anonymous[id]);
        }

        
        /// <summary>
        /// Removes the action/method to the event listeners.
        /// </summary>
        /// <param name="listener">The listener to remove.</param>
        public void Remove(Action<T> listener) 
        {
            Action -= listener;
        }
        
        
        /// <summary>
        /// Removes the action/method from the event listeners.
        /// </summary>
        /// <param name="id">The id of the listener to remove.</param>
        public void RemoveAnonymous(string id)
        {
            if (!anonymous.ContainsKey(id)) return;
            Remove(anonymous[id]);
            anonymous.Remove(id);
        }


        /// <summary>
        /// Clears all listeners from the event.
        /// </summary>
        public void Clear() 
        {
            anonymous.Clear();
            Action = null;
        }
    }
    
    
    /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────────
    |   2 Parameter Evt
    ───────────────────────────────────────────────────────────────────────────────────────────────────────────────── */
    
    
    /// <summary>
    /// A custom event class that helps avoid over subscription and more.
    /// </summary>
    public sealed class Evt<T1,T2>
    {
        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Fields
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */
        
        private readonly Dictionary<string, Action<T1,T2>> anonymous = new Dictionary<string, Action<T1,T2>>();
        private event Action<T1,T2> Action = delegate { };
        
        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Methods
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */
        
        /// <summary>
        /// Raises the event to all listeners.
        /// </summary>
        /// <param name="param1">A param to pass through when raising.</param>
        /// <param name="param2">A param to pass through when raising.</param>
        public void Raise(T1 param1, T2 param2)
        {
            Action?.Invoke(param1, param2);
        }

        
        /// <summary>
        /// Adds the action/method to the event listeners.
        /// </summary>
        /// <param name="listener">The listener to add.</param>
        public void Add(Action<T1,T2> listener)
        {
            Action -= listener;
            Action += listener;
        }
        
        
        /// <summary>
        /// Adds the action/method to the event listeners.
        /// </summary>
        /// <param name="id">The id to refer to this listener.</param>
        /// <param name="listener">The listener to add.</param>
        public void AddAnonymous(string id, Action<T1,T2> listener)
        {
            if (anonymous.TryGetValue(id, out var anon))
            {
                Add(anon);
                return;
            }
            
            anonymous.Add(id, listener);
            Add(anonymous[id]);
        }

        
        /// <summary>
        /// Removes the action/method to the event listeners.
        /// </summary>
        /// <param name="listener">The listener to remove.</param>
        public void Remove(Action<T1,T2> listener) 
        {
            Action -= listener;
        }
        
        
        /// <summary>
        /// Removes the action/method from the event listeners.
        /// </summary>
        /// <param name="id">The id of the listener to remove.</param>
        public void RemoveAnonymous(string id)
        {
            if (!anonymous.ContainsKey(id)) return;
            Remove(anonymous[id]);
            anonymous.Remove(id);
        }
        
        
        /// <summary>
        /// Clears all listeners from the event.
        /// </summary>
        public void Clear() 
        {
            anonymous.Clear();
            Action = null;
        }
    }
    
    
    /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────────
    |   3 Parameter Evt
    ───────────────────────────────────────────────────────────────────────────────────────────────────────────────── */
    
    
    /// <summary>
    /// A custom event class that helps avoid over subscription and more.
    /// </summary>
    public sealed class Evt<T1,T2,T3>
    {
        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Fields
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */
        
        private readonly Dictionary<string, Action<T1,T2,T3>> anonymous = new Dictionary<string, Action<T1,T2,T3>>();
        private event Action<T1,T2,T3> Action = delegate { };
        
        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Methods
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */
        
        /// <summary>
        /// Raises the event to all listeners.
        /// </summary>
        /// <param name="param1">A param to pass through when raising.</param>
        /// <param name="param2">A param to pass through when raising.</param>
        /// <param name="param3">A param to pass through when raising.</param>
        public void Raise(T1 param1, T2 param2, T3 param3)
        {
            Action?.Invoke(param1, param2, param3);
        }

        
        /// <summary>
        /// Adds the action/method to the event listeners.
        /// </summary>
        /// <param name="listener">The listener to add.</param>
        public void Add(Action<T1,T2,T3> listener)
        {
            Action -= listener;
            Action += listener;
        }
        
        
        /// <summary>
        /// Adds the action/method to the event listeners.
        /// </summary>
        /// <param name="id">The id to refer to this listener.</param>
        /// <param name="listener">The listener to add.</param>
        public void AddAnonymous(string id, Action<T1,T2,T3> listener)
        {
            if (anonymous.TryGetValue(id, out var anon))
            {
                Add(anon);
                return;
            }
            
            anonymous.Add(id, listener);
            Add(anonymous[id]);
        }

        
        /// <summary>
        /// Removes the action/method to the event listeners.
        /// </summary>
        /// <param name="listener">The listener to remove.</param>
        public void Remove(Action<T1,T2,T3> listener) 
        {
            Action -= listener;
        }
        
        
        /// <summary>
        /// Removes the action/method from the event listeners.
        /// </summary>
        /// <param name="id">The id of the listener to remove.</param>
        public void RemoveAnonymous(string id)
        {
            if (!anonymous.ContainsKey(id)) return;
            Remove(anonymous[id]);
            anonymous.Remove(id);
        }
        
        
        /// <summary>
        /// Clears all listeners from the event.
        /// </summary>
        public void Clear() 
        {
            anonymous.Clear();
            Action = null;
        }
    }
    
    
    /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────────
    |   4 Parameter Evt
    ───────────────────────────────────────────────────────────────────────────────────────────────────────────────── */
    
    
    /// <summary>
    /// A custom event class that helps avoid over subscription and more.
    /// </summary>
    public sealed class Evt<T1,T2,T3,T4>
    {
        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Fields
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */
        
        private readonly Dictionary<string, Action<T1,T2,T3,T4>> anonymous = new Dictionary<string, Action<T1,T2,T3,T4>>();
        private event Action<T1,T2,T3,T4> Action = delegate { };
        
        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Methods
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */
        
        /// <summary>
        /// Raises the event to all listeners.
        /// </summary>
        /// <param name="param1">A param to pass through when raising.</param>
        /// <param name="param2">A param to pass through when raising.</param>
        /// <param name="param3">A param to pass through when raising.</param>
        /// <param name="param4">A param to pass through when raising.</param>
        public void Raise(T1 param1, T2 param2, T3 param3, T4 param4)
        {
            Action?.Invoke(param1, param2, param3, param4);
        }

        
        /// <summary>
        /// Adds the action/method to the event listeners.
        /// </summary>
        /// <param name="listener">The listener to add.</param>
        public void Add(Action<T1,T2,T3,T4> listener)
        {
            Action -= listener;
            Action += listener;
        }
        
        
        /// <summary>
        /// Adds the action/method to the event listeners.
        /// </summary>
        /// <param name="id">The id to refer to this listener.</param>
        /// <param name="listener">The listener to add.</param>
        public void AddAnonymous(string id, Action<T1,T2,T3,T4> listener)
        {
            if (anonymous.TryGetValue(id, out var anon))
            {
                Add(anon);
                return;
            }
            
            anonymous.Add(id, listener);
            Add(anonymous[id]);
        }

        
        /// <summary>
        /// Removes the action/method to the event listeners.
        /// </summary>
        /// <param name="listener">The listener to remove.</param>
        public void Remove(Action<T1,T2,T3,T4> listener) 
        {
            Action -= listener;
        }
        
        
        /// <summary>
        /// Removes the action/method from the event listeners.
        /// </summary>
        /// <param name="id">The id of the listener to remove.</param>
        public void RemoveAnonymous(string id)
        {
            if (!anonymous.ContainsKey(id)) return;
            Remove(anonymous[id]);
            anonymous.Remove(id);
        }
        
        
        /// <summary>
        /// Clears all listeners from the event.
        /// </summary>
        public void Clear() 
        {
            anonymous.Clear();
            Action = null;
        }
    }


    /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────────
    |   5 Parameter Evt
    ───────────────────────────────────────────────────────────────────────────────────────────────────────────────── */
    
    
    /// <summary>
    /// A custom event class that helps avoid over subscription and more.
    /// </summary>
    public sealed class Evt<T1,T2,T3,T4,T5>
    {
        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Fields
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */
        
        private readonly Dictionary<string, Action<T1,T2,T3,T4,T5>> anonymous = new Dictionary<string, Action<T1,T2,T3,T4,T5>>();
        private event Action<T1,T2,T3,T4,T5> Action = delegate { };
        
        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Methods
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */
        
        /// <summary>
        /// Raises the event to all listeners.
        /// </summary>
        /// <param name="param1">A param to pass through when raising.</param>
        /// <param name="param2">A param to pass through when raising.</param>
        /// <param name="param3">A param to pass through when raising.</param>
        /// <param name="param4">A param to pass through when raising.</param>
        /// <param name="param5">A param to pass through when raising.</param>
        public void Raise(T1 param1, T2 param2, T3 param3, T4 param4, T5 param5)
        {
            Action?.Invoke(param1, param2, param3, param4, param5);
        }

        
        /// <summary>
        /// Adds the action/method to the event listeners.
        /// </summary>
        /// <param name="listener">The listener to add.</param>
        public void Add(Action<T1,T2,T3,T4,T5> listener)
        {
            Action -= listener;
            Action += listener;
        }
        
        
        /// <summary>
        /// Adds the action/method to the event listeners.
        /// </summary>
        /// <param name="id">The id to refer to this listener.</param>
        /// <param name="listener">The listener to add.</param>
        public void AddAnonymous(string id, Action<T1,T2,T3,T4,T5> listener)
        {
            if (anonymous.TryGetValue(id, out var anon))
            {
                Add(anon);
                return;
            }
            
            anonymous.Add(id, listener);
            Add(anonymous[id]);
        }

        
        /// <summary>
        /// Removes the action/method to the event listeners.
        /// </summary>
        /// <param name="listener">The listener to remove.</param>
        public void Remove(Action<T1,T2,T3,T4,T5> listener) 
        {
            Action -= listener;
        }
        
        
        /// <summary>
        /// Removes the action/method from the event listeners.
        /// </summary>
        /// <param name="id">The id of the listener to remove.</param>
        public void RemoveAnonymous(string id)
        {
            if (!anonymous.ContainsKey(id)) return;
            Remove(anonymous[id]);
            anonymous.Remove(id);
        }
        
        
        /// <summary>
        /// Clears all listeners from the event.
        /// </summary>
        public void Clear() 
        {
            anonymous.Clear();
            Action = null;
        }
    }
    

    /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────────
    |   6 Parameter Evt
    ───────────────────────────────────────────────────────────────────────────────────────────────────────────────── */
    
    
    /// <summary>
    /// A custom event class that helps avoid over subscription and more.
    /// </summary>
    public sealed class Evt<T1,T2,T3,T4,T5,T6>
    {
        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Fields
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */
        
        private readonly Dictionary<string, Action<T1,T2,T3,T4,T5,T6>> anonymous = new Dictionary<string, Action<T1,T2,T3,T4,T5,T6>>();
        private event Action<T1,T2,T3,T4,T5,T6> Action = delegate { };
        
        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Methods
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */
        
        /// <summary>
        /// Raises the event to all listeners.
        /// </summary>
        /// <param name="param1">A param to pass through when raising.</param>
        /// <param name="param2">A param to pass through when raising.</param>
        /// <param name="param3">A param to pass through when raising.</param>
        /// <param name="param4">A param to pass through when raising.</param>
        /// <param name="param5">A param to pass through when raising.</param>
        /// <param name="param6">A param to pass through when raising.</param>
        public void Raise(T1 param1, T2 param2, T3 param3, T4 param4, T5 param5, T6 param6)
        {
            Action?.Invoke(param1, param2, param3, param4, param5, param6);
        }

        
        /// <summary>
        /// Adds the action/method to the event listeners.
        /// </summary>
        /// <param name="listener">The listener to add.</param>
        public void Add(Action<T1,T2,T3,T4,T5,T6> listener)
        {
            Action -= listener;
            Action += listener;
        }
        
        
        /// <summary>
        /// Adds the action/method to the event listeners.
        /// </summary>
        /// <param name="id">The id to refer to this listener.</param>
        /// <param name="listener">The listener to add.</param>
        public void AddAnonymous(string id, Action<T1,T2,T3,T4,T5,T6> listener)
        {
            if (anonymous.TryGetValue(id, out var anon))
            {
                Add(anon);
                return;
            }
            
            anonymous.Add(id, listener);
            Add(anonymous[id]);
        }

        
        /// <summary>
        /// Removes the action/method to the event listeners.
        /// </summary>
        /// <param name="listener">The listener to remove.</param>
        public void Remove(Action<T1,T2,T3,T4,T5,T6> listener) 
        {
            Action -= listener;
        }
        
        
        /// <summary>
        /// Removes the action/method from the event listeners.
        /// </summary>
        /// <param name="id">The id of the listener to remove.</param>
        public void RemoveAnonymous(string id)
        {
            if (!anonymous.ContainsKey(id)) return;
            Remove(anonymous[id]);
            anonymous.Remove(id);
        }
        
        
        /// <summary>
        /// Clears all listeners from the event.
        /// </summary>
        public void Clear() 
        {
            anonymous.Clear();
            Action = null;
        }
    }
    

    /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────────
    |   7 Parameter Evt
    ───────────────────────────────────────────────────────────────────────────────────────────────────────────────── */
    
    
    /// <summary>
    /// A custom event class that helps avoid over subscription and more.
    /// </summary>
    public sealed class Evt<T1,T2,T3,T4,T5,T6,T7>
    {
        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Fields
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */
        
        private readonly Dictionary<string, Action<T1,T2,T3,T4,T5,T6,T7>> anonymous = new Dictionary<string, Action<T1,T2,T3,T4,T5,T6,T7>>();
        private event Action<T1,T2,T3,T4,T5,T6,T7> Action = delegate { };
        
        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Methods
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */
        
        /// <summary>
        /// Raises the event to all listeners.
        /// </summary>
        /// <param name="param1">A param to pass through when raising.</param>
        /// <param name="param2">A param to pass through when raising.</param>
        /// <param name="param3">A param to pass through when raising.</param>
        /// <param name="param4">A param to pass through when raising.</param>
        /// <param name="param5">A param to pass through when raising.</param>
        /// <param name="param6">A param to pass through when raising.</param>
        /// <param name="param7">A param to pass through when raising.</param>
        public void Raise(T1 param1, T2 param2, T3 param3, T4 param4, T5 param5, T6 param6, T7 param7)
        {
            Action?.Invoke(param1, param2, param3, param4, param5, param6, param7);
        }

        
        /// <summary>
        /// Adds the action/method to the event listeners.
        /// </summary>
        /// <param name="listener">The listener to add.</param>
        public void Add(Action<T1,T2,T3,T4,T5,T6,T7> listener)
        {
            Action -= listener;
            Action += listener;
        }
        
        
        /// <summary>
        /// Adds the action/method to the event listeners.
        /// </summary>
        /// <param name="id">The id to refer to this listener.</param>
        /// <param name="listener">The listener to add.</param>
        public void AddAnonymous(string id, Action<T1,T2,T3,T4,T5,T6,T7> listener)
        {
            if (anonymous.TryGetValue(id, out var anon))
            {
                Add(anon);
                return;
            }
            
            anonymous.Add(id, listener);
            Add(anonymous[id]);
        }

        
        /// <summary>
        /// Removes the action/method to the event listeners.
        /// </summary>
        /// <param name="listener">The listener to remove.</param>
        public void Remove(Action<T1,T2,T3,T4,T5,T6,T7> listener) 
        {
            Action -= listener;
        }
        
        
        /// <summary>
        /// Removes the action/method from the event listeners.
        /// </summary>
        /// <param name="id">The id of the listener to remove.</param>
        public void RemoveAnonymous(string id)
        {
            if (!anonymous.ContainsKey(id)) return;
            Remove(anonymous[id]);
            anonymous.Remove(id);
        }
        
        
        /// <summary>
        /// Clears all listeners from the event.
        /// </summary>
        public void Clear() 
        {
            anonymous.Clear();
            Action = null;
        }
    }
    
    
    /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────────
    |   8 Parameter Evt
    ───────────────────────────────────────────────────────────────────────────────────────────────────────────────── */
    
    
    /// <summary>
    /// A custom event class that helps avoid over subscription and more.
    /// </summary>
    public sealed class Evt<T1,T2,T3,T4,T5,T6,T7,T8>
    {
        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Fields
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */
        
        private readonly Dictionary<string, Action<T1,T2,T3,T4,T5,T6,T7,T8>> anonymous = new Dictionary<string, Action<T1,T2,T3,T4,T5,T6,T7,T8>>();
        private event Action<T1,T2,T3,T4,T5,T6,T7,T8> Action = delegate { };
        
        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Methods
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */
        
        /// <summary>
        /// Raises the event to all listeners.
        /// </summary>
        /// <param name="param1">A param to pass through when raising.</param>
        /// <param name="param2">A param to pass through when raising.</param>
        /// <param name="param3">A param to pass through when raising.</param>
        /// <param name="param4">A param to pass through when raising.</param>
        /// <param name="param5">A param to pass through when raising.</param>
        /// <param name="param6">A param to pass through when raising.</param>
        /// <param name="param7">A param to pass through when raising.</param>
        /// <param name="param8">A param to pass through when raising.</param>
        public void Raise(T1 param1, T2 param2, T3 param3, T4 param4, T5 param5, T6 param6, T7 param7, T8 param8)
        {
            Action?.Invoke(param1, param2, param3, param4, param5, param6, param7, param8);
        }

        
        /// <summary>
        /// Adds the action/method to the event listeners.
        /// </summary>
        /// <param name="listener">The listener to add.</param>
        public void Add(Action<T1,T2,T3,T4,T5,T6,T7,T8> listener)
        {
            Action -= listener;
            Action += listener;
        }
        
        
        /// <summary>
        /// Adds the action/method to the event listeners.
        /// </summary>
        /// <param name="id">The id to refer to this listener.</param>
        /// <param name="listener">The listener to add.</param>
        public void AddAnonymous(string id, Action<T1,T2,T3,T4,T5,T6,T7,T8> listener)
        {
            if (anonymous.TryGetValue(id, out var anon))
            {
                Add(anon);
                return;
            }
            
            anonymous.Add(id, listener);
            Add(anonymous[id]);
        }

        
        /// <summary>
        /// Removes the action/method to the event listeners.
        /// </summary>
        /// <param name="listener">The listener to remove.</param>
        public void Remove(Action<T1,T2,T3,T4,T5,T6,T7,T8> listener) 
        {
            Action -= listener;
        }
        
        
        /// <summary>
        /// Removes the action/method from the event listeners.
        /// </summary>
        /// <param name="id">The id of the listener to remove.</param>
        public void RemoveAnonymous(string id)
        {
            if (!anonymous.ContainsKey(id)) return;
            Remove(anonymous[id]);
            anonymous.Remove(id);
        }
        
        
        /// <summary>
        /// Clears all listeners from the event.
        /// </summary>
        public void Clear() 
        {
            anonymous.Clear();
            Action = null;
        }
    }
}