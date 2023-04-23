/*
 * Copyright (c) 2018-Present Carter Games
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

namespace CarterGames.Common
{
    /// <summary>
    /// An event class with no parameters...
    /// </summary>
    public sealed class Evt
    {
        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Fields
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */
        
        /// <summary>
        /// The action to use for the evt.
        /// </summary>
        private event Action Action = delegate { };
        
        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Methods
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */
        
        /// <summary>
        /// Raises the event to all listeners.
        /// </summary>
        public void Raise() => Action.Invoke();
        

        /// <summary>
        /// Adds the action/method to the event listeners.
        /// </summary>
        public void Add(Action listener)
        {
            Action -= listener;
            Action += listener;
        }
        

        /// <summary>
        /// Removes the action/method to the event listeners.
        /// </summary>
        public void Remove(Action listener) => Action -= listener;
        
        
        /// <summary>
        /// Clears all listeners from the event.
        /// </summary>
        public void Clear() => Action = null;
    }
    
    
    /// <summary>
    /// An event class with 1 parameter...
    /// </summary>
    public sealed class Evt<T>
    {
        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Fields
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */
        
        /// <summary>
        /// The action to use for the evt.
        /// </summary>
        private event Action<T> Action = delegate { };

        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Methods
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */
        
        /// <summary>
        /// Raises the event to all listeners.
        /// </summary>
        public void Raise(T param) => Action.Invoke(param);

        
        /// <summary>
        /// Adds the action/method to the event listeners.
        /// </summary>
        public void Add(Action<T> listener)
        {
            Action -= listener;
            Action += listener;
        }

        
        /// <summary>
        /// Removes the action/method to the event listeners.
        /// </summary>
        public void Remove(Action<T> listener) => Action -= listener;

        
        /// <summary>
        /// Clears all listeners from the event.
        /// </summary>
        public void Clear() => Action = null;
    }
    
    
    /// <summary>
    /// An event class with 2 parameters...
    /// </summary>
    public sealed class Evt<T1,T2>
    {
        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Fields
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */
        
        /// <summary>
        /// The action to use for the evt.
        /// </summary>
        private event Action<T1,T2> Action = delegate { };

        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Methods
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */
        
        /// <summary>
        /// Raises the event to all listeners.
        /// </summary>
        public void Raise(T1 param1, T2 param2) => Action.Invoke(param1, param2);

        
        /// <summary>
        /// Adds the action/method to the event listeners.
        /// </summary>
        public void Add(Action<T1,T2> listener)
        {
            Action -= listener;
            Action += listener;
        }
        

        /// <summary>
        /// Removes the action/method to the event listeners.
        /// </summary>
        public void Remove(Action<T1,T2> listener) => Action -= listener;
        
        
        /// <summary>
        /// Clears all listeners from the event.
        /// </summary>
        public void Clear() => Action = null;
    }
    
    
    /// <summary>
    /// An event class with 3 parameters...
    /// </summary>
    public sealed class Evt<T1,T2,T3>
    {
        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Fields
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */
        
        /// <summary>
        /// The action to use for the evt.
        /// </summary>
        private event Action<T1,T2,T3> Action = delegate { };

        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Methods
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */
        
        /// <summary>
        /// Raises the event to all listeners.
        /// </summary>
        public void Raise(T1 param1, T2 param2, T3 param3) => Action.Invoke(param1, param2, param3);

        
        /// <summary>
        /// Adds the action/method to the event listeners.
        /// </summary>
        public void Add(Action<T1,T2,T3> listener)
        {
            Action -= listener;
            Action += listener;
        }

        
        /// <summary>
        /// Removes the action/method to the event listeners.
        /// </summary>
        public void Remove(Action<T1,T2,T3> listener) => Action -= listener;
        
        
        /// <summary>
        /// Clears all listeners from the event.
        /// </summary>
        public void Clear() => Action = null;
    }
    
    
    /// <summary>
    /// An event class with 4 parameters...
    /// </summary>
    public sealed class Evt<T1,T2,T3,T4>
    {
        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Fields
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */
        
        /// <summary>
        /// The action to use for the evt.
        /// </summary>
        private event Action<T1,T2,T3,T4> Action = delegate { };

        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Methods
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */
        
        /// <summary>
        /// Raises the event to all listeners.
        /// </summary>
        public void Raise(T1 param1, T2 param2, T3 param3, T4 param4) => Action.Invoke(param1, param2, param3, param4);

        
        /// <summary>
        /// Adds the action/method to the event listeners.
        /// </summary>
        public void Add(Action<T1,T2,T3,T4> listener)
        {
            Action -= listener;
            Action += listener;
        }

        
        /// <summary>
        /// Removes the action/method to the event listeners.
        /// </summary>
        public void Remove(Action<T1,T2,T3,T4> listener) => Action -= listener;
        
        
        /// <summary>
        /// Clears all listeners from the event.
        /// </summary>
        public void Clear() => Action = null;
    }

    
    /// <summary>
    /// An event class with 5 parameters...
    /// </summary>
    public sealed class Evt<T1,T2,T3,T4,T5>
    {
        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Fields
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */
        
        /// <summary>
        /// The action to use for the evt.
        /// </summary>
        private event Action<T1,T2,T3,T4,T5> Action = delegate { };

        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Methods
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */
        
        /// <summary>
        /// Raises the event to all listeners.
        /// </summary>
        public void Raise(T1 param1, T2 param2, T3 param3, T4 param4, T5 param5) 
            => Action.Invoke(param1, param2, param3, param4, param5);

        
        /// <summary>
        /// Adds the action/method to the event listeners.
        /// </summary>
        public void Add(Action<T1,T2,T3,T4,T5> listener)
        {
            Action -= listener;
            Action += listener;
        }

        
        /// <summary>
        /// Removes the action/method to the event listeners.
        /// </summary>
        public void Remove(Action<T1,T2,T3,T4,T5> listener) => Action -= listener;
        
        
        /// <summary>
        /// Clears all listeners from the event.
        /// </summary>
        public void Clear() => Action = null;
    }
    
    
    /// <summary>
    /// An event class with 6 parameters...
    /// </summary>
    public sealed class Evt<T1,T2,T3,T4,T5,T6>
    {
        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Fields
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */
        
        /// <summary>
        /// The action to use for the evt.
        /// </summary>
        private event Action<T1,T2,T3,T4,T5,T6> Action = delegate { };

        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Methods
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */
        
        /// <summary>
        /// Raises the event to all listeners.
        /// </summary>
        public void Raise(T1 param1, T2 param2, T3 param3, T4 param4, T5 param5, T6 param6) 
            => Action.Invoke(param1, param2, param3, param4, param5, param6);

        
        /// <summary>
        /// Adds the action/method to the event listeners.
        /// </summary>
        public void Add(Action<T1,T2,T3,T4,T5,T6> listener)
        {
            Action -= listener;
            Action += listener;
        }

        
        /// <summary>
        /// Removes the action/method to the event listeners.
        /// </summary>
        public void Remove(Action<T1,T2,T3,T4,T5,T6> listener) => Action -= listener;
        
        
        /// <summary>
        /// Clears all listeners from the event.
        /// </summary>
        public void Clear() => Action = null;
    }
    
    
    /// <summary>
    /// An event class with 7 parameters...
    /// </summary>
    public sealed class Evt<T1,T2,T3,T4,T5,T6,T7>
    {
        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Fields
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */
        
        /// <summary>
        /// The action to use for the evt.
        /// </summary>
        private event Action<T1,T2,T3,T4,T5,T6,T7> Action = delegate { };

        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Methods
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */
        
        /// <summary>
        /// Raises the event to all listeners.
        /// </summary>
        public void Raise(T1 param1, T2 param2, T3 param3, T4 param4, T5 param5, T6 param6, T7 param7) 
            => Action.Invoke(param1, param2, param3, param4, param5, param6, param7);

        
        /// <summary>
        /// Adds the action/method to the event listeners.
        /// </summary>
        public void Add(Action<T1,T2,T3,T4,T5,T6,T7> listener)
        {
            Action -= listener;
            Action += listener;
        }

        
        /// <summary>
        /// Removes the action/method to the event listeners.
        /// </summary>
        public void Remove(Action<T1,T2,T3,T4,T5,T6,T7> listener) => Action -= listener;
        
        
        /// <summary>
        /// Clears all listeners from the event.
        /// </summary>
        public void Clear() => Action = null;
    }
    
    
    /// <summary>
    /// An event class with 8 parameters...
    /// </summary>
    public sealed class Evt<T1,T2,T3,T4,T5,T6,T7,T8>
    {
        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Fields
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */
        
        /// <summary>
        /// The action to use for the evt.
        /// </summary>
        private event Action<T1,T2,T3,T4,T5,T6,T7,T8> Action = delegate { };

        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Methods
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */
        
        /// <summary>
        /// Raises the event to all listeners.
        /// </summary>
        public void Raise(T1 param1, T2 param2, T3 param3, T4 param4, T5 param5, T6 param6, T7 param7, T8 param8) 
            => Action.Invoke(param1, param2, param3, param4, param5, param6, param7, param8);

        
        /// <summary>
        /// Adds the action/method to the event listeners.
        /// </summary>
        public void Add(Action<T1,T2,T3,T4,T5,T6,T7,T8> listener)
        {
            Action -= listener;
            Action += listener;
        }

        
        /// <summary>
        /// Removes the action/method to the event listeners.
        /// </summary>
        public void Remove(Action<T1,T2,T3,T4,T5,T6,T7,T8> listener) => Action -= listener;
        
        
        /// <summary>
        /// Clears all listeners from the event.
        /// </summary>
        public void Clear() => Action = null;
    }
}