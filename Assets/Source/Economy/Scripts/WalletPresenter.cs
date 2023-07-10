using UnityEngine;

    public class WalletPresenter : MonoBehaviour
    {
        [SerializeField] protected int StartValue;
        [SerializeField] private WalletView _view;

        private Wallet _wallet;

        public int Value => _wallet.Value;

        private void OnEnable()
        {
            _wallet = new Wallet(StartValue, nameof(Wallet));
            _wallet.Load();
            _view.Render(_wallet.Value);
            _wallet.ValueChanged += OnValueChanged;
        }

        private void OnDisable() => _wallet.ValueChanged -= OnValueChanged;

        public void SpendResource(int value) => _wallet.Spend(value);

        public void AddResource(int value) => _wallet.Add(value);

        private void OnValueChanged()
        {
            _wallet.Save();
            _view.Render(_wallet.Value);
        }
}
