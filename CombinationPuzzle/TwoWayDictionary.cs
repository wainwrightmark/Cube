using System.Collections.Generic;
using System.Linq;

namespace CombinationPuzzle
{

    public class ReadonlyTwoWayDictionary<T1, T2> where T1 : struct where T2 : struct
    {

        public IReadOnlyDictionary<T1, T2> Forwards { get; }
        public IReadOnlyDictionary<T2, T1> Backwards { get; }

        public IEnumerable<T1> Set1 => Forwards.Keys;
        public IEnumerable<T2> Set2 => Backwards.Keys;


        public ReadonlyTwoWayDictionary(params (T1 t1, T2 t2) [] initial)
        {
            Forwards = initial.ToDictionary(x => x.t1, x => x.t2);
            Backwards = initial.ToDictionary(kvp => kvp.t2, kvp => kvp.t1);
        }

        public ReadonlyTwoWayDictionary(IReadOnlyCollection<KeyValuePair<T1, T2>> initial)
        {
            Forwards = initial.ToDictionary(x => x.Key, x => x.Value);
            Backwards = initial.ToDictionary(kvp => kvp.Value, kvp => kvp.Key);
        }


        public ReadonlyTwoWayDictionary(IDictionary<T1, T2> initial)
        {
            Forwards = initial.ToDictionary(x=>x.Key, x=>x.Value);
            Backwards = initial.ToDictionary(kvp => kvp.Value, kvp => kvp.Key);
        }

        public int Count => Forwards.Count;

        public bool Contains(T1 item) => Forwards.ContainsKey(item);
        public bool Contains(T2 item) => Backwards.ContainsKey(item);



        public T1 this[T2 index] => Backwards[index];

        public T2 this[T1 index] => Forwards[index];
    }


    public class TwoWayDictionary<T1, T2> where T1 : struct where T2 : struct
    {
        private readonly Dictionary<T1, T2> _forwards;
        private readonly Dictionary<T2, T1> _backwards;

        public IReadOnlyDictionary<T1, T2> Forwards => _forwards;
        public IReadOnlyDictionary<T2, T1> Backwards => _backwards;

        public IEnumerable<T1> Set1 => Forwards.Keys;
        public IEnumerable<T2> Set2 => Backwards.Keys;


        public TwoWayDictionary()
        {
            _forwards = new Dictionary<T1, T2>();
            _backwards = new Dictionary<T2, T1>();
        }

        public TwoWayDictionary(int capacity)
        {
            _forwards = new Dictionary<T1, T2>(capacity);
            _backwards = new Dictionary<T2, T1>(capacity);
        }

        public TwoWayDictionary(Dictionary<T1, T2> initial)
        {
            _forwards = initial;
            _backwards = initial.ToDictionary(kvp => kvp.Value, kvp => kvp.Key);
        }

        public TwoWayDictionary(Dictionary<T2, T1> initial)
        {
            _backwards = initial;
            _forwards = initial.ToDictionary(kvp => kvp.Value, kvp => kvp.Key);
        }


        public T1 this[T2 index]
        {
            get => _backwards[index];
            set
            {
                if (_backwards.TryGetValue(index, out var removeThis))
                    _forwards.Remove(removeThis);

                _backwards[index] = value;
                _forwards[value] = index;
            }
        }

        public T2 this[T1 index]
        {
            get => _forwards[index];
            set
            {
                if (_forwards.TryGetValue(index, out var removeThis))
                    _backwards.Remove(removeThis);

                _forwards[index] = value;
                _backwards[value] = index;
            }
        }

        public int Count => _forwards.Count;

        public bool Contains(T1 item) => _forwards.ContainsKey(item);
        public bool Contains(T2 item) => _backwards.ContainsKey(item);

        public bool Remove(T1 item)
        {
            if (!Contains(item))
                return false;

            var t2 = _forwards[item];

            _backwards.Remove(t2);
            _forwards.Remove(item);

            return true;
        }

        public bool Remove(T2 item)
        {
            if (!Contains(item))
                return false;

            var t1 = _backwards[item];

            _forwards.Remove(t1);
            _backwards.Remove(item);

            return true;
        }

        public void Clear()
        {
            _forwards.Clear();
            _backwards.Clear();
        }
    }
}